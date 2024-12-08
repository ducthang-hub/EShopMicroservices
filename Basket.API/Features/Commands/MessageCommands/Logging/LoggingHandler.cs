using System.Net;
using BuildingBlocks.CQRS;
using BuildingBlocks.MessageQueue.Producer;
using BuildingBlocks.MessageQueue.Requests;
using RabbitMQ.Client;

namespace Basket.API.Features.Commands.MessageCommands.Logging;

public class LoggingHandler(IProducer producer, ILogger<LoggingHandler> logger) : ICommandHandler<LoggingCommand, LoggingResponse>
{
    private const string QueueName = "Monitoring";
    public Task<LoggingResponse> Handle(LoggingCommand request, CancellationToken cancellationToken)
    {
        
        var response = new LoggingResponse();

        try
        {
            var routingKeys = request.LoggingRequest.Severities;
            foreach (var routingKey in routingKeys)
            {
                var publishRequest = new PublishRequest
                {
                    Exchange = QueueName,
                    ExchangeType = ExchangeType.Direct,
                    Message = request.LoggingRequest.Message,
                    RoutingKey = routingKey
                };
                producer.PublishMessage(publishRequest, cancellationToken);
            }
            
            response.Status = HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error {ex.Message}");
        }

        return Task.FromResult(response);
    }
}