using System.Text;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BuildingBlocks.MessageQueue.Consumer;

public class BasicConsumer(IMessageQueueConnectionProvider connectionProvider, ILogger<BasicConsumer> logger) : IConsumer
{
    public async Task ConsumeMessages(string queue, string consumedBy, CancellationToken cancellationToken)
    {
        try
        {
            var queueName = queue.ToLowerInvariant();
            
            var connection = await connectionProvider.GetConnection();
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await HandleMessage(message, consumedBy, logger);
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken);
            };

            await channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error: {ex.Message}");
        }
    }

    private async Task HandleMessage(string message, string consumedBy, ILogger<BasicConsumer> logger)
    {
        logger.LogInformation($" [x] Consumer by {consumedBy} Received {message}");
                
        int dots = message.Split('.').Length - 1;
        await Task.Delay(dots * 1000);

        logger.LogInformation($" [x] Consumer by {consumedBy} Done");
    }
}