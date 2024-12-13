using System.Text;
using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BuildingBlocks.Protocols.Rpc.RpcServer;

public class RpcServer<T>(IMessageQueueConnectionProvider connectionProvider, ILogger logger, string queueName) : IRpcServer
{
    public async Task ConsumeMessages(object messageRequest, CancellationToken cancellationToken)
    {
        try
        {
            var connection = await connectionProvider.GetConnection();
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);

            // todo: accept only one message per handling round
            
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var coupons = new List<T>();
                var couponsAsString = JsonHelper.Serialize(coupons);
                var body = Encoding.UTF8.GetBytes(couponsAsString);

                var replyQueue = ea.BasicProperties.ReplyTo;

                if (string.IsNullOrEmpty(replyQueue))
                {
                    logger.LogWarning("Cannot find reply queue");
                    return;
                }
                var replyProperties = new BasicProperties
                {
                    CorrelationId = ea.BasicProperties.CorrelationId
                };

                var senderConsumer = (AsyncEventingBasicConsumer)sender;
                var senderChannel = senderConsumer.Channel;
    
                await senderChannel.BasicPublishAsync(exchange: string.Empty, routingKey: replyQueue, mandatory: true,
                    basicProperties: replyProperties, body: body, cancellationToken: cancellationToken);
                
                //todo: acknowledge to client that server receives the message
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