using System.Text;
using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BuildingBlocks.MessageQueue.Producer;

public class Producer(IMessageQueueConnectionProvider connectionProvider, ILogger<Producer> logger) : IProducer
{
    public async Task PublishMessage(string exchange, string message, CancellationToken cancellationToken)
    {
        try
        {
            var connection = await connectionProvider.GetConnection();
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(exchange: exchange, ExchangeType.Fanout, cancellationToken: cancellationToken);
            await channel.BasicPublishAsync(exchange: exchange, routingKey: string.Empty, body: TransformMessage(message), cancellationToken: cancellationToken);
            
            logger.LogInformation($" [x] Sent {message}");
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }

    private static byte[] TransformMessage(string message)
    {
        return Encoding.UTF8.GetBytes(message);

    }
}