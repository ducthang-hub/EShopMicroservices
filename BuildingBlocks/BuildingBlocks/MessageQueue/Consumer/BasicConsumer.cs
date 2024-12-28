using System.Text;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using BuildingBlocks.MessageQueue.Requests;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BuildingBlocks.MessageQueue.Consumer;

public class BasicConsumer(IMessageQueueConnectionProvider connectionProvider, ILogger<BasicConsumer> logger) : IConsumer
{
    public Consumer.HandleMessageFunc HandleMessage { get; set; }

    public async Task ConsumeMessages(ConsumeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var exchange = request.Exchange;
            
            var connection = await connectionProvider.GetConnection();
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(exchange, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await HandleMessage(message);
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken);
            };

            await channel.BasicConsumeAsync(exchange, autoAck: false, consumer: consumer, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error: {ex.Message}");
        }
    }


}