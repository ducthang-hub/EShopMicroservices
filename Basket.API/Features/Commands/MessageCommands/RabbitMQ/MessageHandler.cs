using System.Net;
using BuildingBlocks.CQRS;
using BuildingBlocks.MessageQueue.Producer;
using BuildingBlocks.MessageQueue.Requests;
using RabbitMQ.Client;

namespace Basket.API.Features.Commands.MessageCommands.RabbitMQ;

public class MessageHandler(ILogger<MessageHandler> logger, IProducer producer) : ICommandHandler<MessageCommand, MessageResponse>
{
    private const string QueueName = "Logs";
    public async Task<MessageResponse> Handle(MessageCommand request, CancellationToken cancellationToken)
    {
        
        var response = new MessageResponse();
        var publishRequest = new PublishRequest
        {
            Exchange = QueueName,
            ExchangeType = ExchangeType.Fanout,
            Message = request.Message,
            RoutingKey = string.Empty
        };
        try
        {
            await producer.PublishMessage(publishRequest, cancellationToken);
            response.Status = HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error {ex.Message}");
        }

        return response;
    }
}