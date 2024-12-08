using System.Text;
using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using BuildingBlocks.MessageQueue.Requests;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BuildingBlocks.MessageQueue.Producer;

public class BasicProducer(IMessageQueueConnectionProvider connectionProvider, ILogger<BasicProducer> logger) : IProducer
{
    public async Task PublishMessage(PublishRequest request, CancellationToken cancellationToken)
    {
        try
        {
            
            var connection = await connectionProvider.GetConnection(); 
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
            await channel.QueueDeclareAsync(request.Exchange, durable: true, exclusive: false, autoDelete: false,
                arguments: null, cancellationToken: cancellationToken);
            
            var body = Encoding.UTF8.GetBytes(request.Message);
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: request.Exchange, body: body, cancellationToken: cancellationToken);
            logger.LogInformation($" [x] Sent {request.Message}");
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }
}