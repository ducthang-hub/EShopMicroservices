using System.Net;
using BuildingBlocks.CQRS;
using BuildingBlocks.MessageQueue.Producer;

namespace Basket.API.Features.Commands.MessageCommands.RabbitMQ;

public class MessageHandler(ILogger<MessageHandler> logger, IProducer producer) : ICommandHandler<MessageCommand, MessageResponse>
{
    private const string QueueName = "Logs";
    public async Task<MessageResponse> Handle(MessageCommand request, CancellationToken cancellationToken)
    {
        
        var response = new MessageResponse();
        var message = request.Message;
        try
        {
            await producer.PublishMessage(QueueName, message, cancellationToken);
            response.Status = HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error {ex.Message}");
        }

        return response;
    }
}