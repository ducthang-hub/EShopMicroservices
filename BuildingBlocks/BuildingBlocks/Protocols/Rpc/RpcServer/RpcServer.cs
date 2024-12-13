using System.Text;
using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BuildingBlocks.Protocols.Rpc.RpcServer;

public class RpcServer<T>(IMessageQueueConnectionProvider connectionProvider, ILogger<RpcServer<T>> logger) : IRpcServer<T>
{
    public async Task ConsumeMessages(string queueName, Func<T> handler, CancellationToken cancellationToken)
    {
        try
        {
            var connection = await connectionProvider.GetConnection();
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);

            // accept only one message per handling round
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken);
            
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var replyQueue = ea.BasicProperties.ReplyTo;
                if (string.IsNullOrEmpty(replyQueue))
                {
                    logger.LogWarning("Cannot find reply queue");
                    return;
                }
                
                var data = handler();
                if (data != null)
                {
                    var dataAsString = JsonHelper.Serialize(data);
                    var body = Encoding.UTF8.GetBytes(dataAsString);

                    var replyProperties = new BasicProperties
                    {
                        CorrelationId = ea.BasicProperties.CorrelationId
                    };
                    
                    var senderConsumer = (AsyncEventingBasicConsumer)sender;
                    var senderChannel = senderConsumer.Channel;
                    await senderChannel.BasicPublishAsync(exchange: string.Empty, routingKey: replyQueue, mandatory: true,
                        basicProperties: replyProperties, body: body, cancellationToken: cancellationToken);
                }

                // return acknowledge to client that server receive the message
                await channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
            };

            await channel.BasicConsumeAsync(queueName, false, consumer, cancellationToken);
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }
}