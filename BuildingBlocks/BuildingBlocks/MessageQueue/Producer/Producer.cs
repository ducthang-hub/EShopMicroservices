using System.Text;
using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using BuildingBlocks.MessageQueue.Requests;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BuildingBlocks.MessageQueue.Producer;

public class Producer(IMessageQueueConnectionProvider connectionProvider, ILogger<Producer> logger) : IProducer
{
    public async Task PublishMessage(PublishRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var exchange = request.Exchange;
            var exchangeType = request.ExchangeType;
            var routingKey = request.RoutingKey;
            var message = request.Message;
            
            var connection = await connectionProvider.GetConnection();
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(exchange: exchange, exchangeType, cancellationToken: cancellationToken);
            await channel.BasicPublishAsync(exchange: exchange, routingKey: routingKey, body: TransformMessage(request.Message, routingKey), cancellationToken: cancellationToken);
            
            logger.LogInformation($" [x] Sent {routingKey} {message}");
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }

    private static byte[] TransformMessage(string message, string routingKey)
    {
        return Encoding.UTF8.GetBytes($"{routingKey} {message}");

    }
}