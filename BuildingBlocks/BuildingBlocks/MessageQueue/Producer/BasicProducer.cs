using System.Text;
using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BuildingBlocks.MessageQueue.Producer;

public class BasicProducer(IMessageQueueConnectionProvider connectionProvider, ILogger<BasicProducer> logger) : IProducer
{
    public async Task PublishMessage(string queue, string message, CancellationToken cancellationToken)
    {
        try
        {
            var queueName = queue.ToLowerInvariant();
            
            var connection = await connectionProvider.GetConnection(); 
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false,
                arguments: null, cancellationToken: cancellationToken);
            
            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body, cancellationToken: cancellationToken);
            logger.LogInformation($" [x] Sent {message}");
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }
}