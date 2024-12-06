using System.Text;
using BuildingBlocks.CQRS;
using RabbitMQ.Client;

namespace Basket.API.Features.Commands.MessageCommands.RabbitMQ;

public class MessageHandler(ILogger<MessageHandler> logger) : ICommandHandler<MessageCommand, MessageResponse>
{
    public async Task<MessageResponse> Handle(MessageCommand request, CancellationToken cancellationToken)
    {
        var response = new MessageResponse();
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                Password = "guest",
                UserName = "guest"
            };
            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
                arguments: null, cancellationToken: cancellationToken);
            
            const string message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body, cancellationToken: cancellationToken);
            logger.LogInformation($" [x] Sent {message}");

            logger.LogInformation(" Press [enter] to exit.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error {ex.Message}");
        }

        return response;
    }
}