using BuildingBlock.Messaging.IntegrationEvents;
using BuildingBlocks.MassTransit.Contracts.Queues;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ordering.Domain.Events;

namespace Ordering.Application.Features.EventHandlers.DomainEvents;

public class CreateOrderEventHandler : INotificationHandler<CreatedOrderEvent>
{
    private readonly ILogger<CreateOrderEventHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint; 
    public CreateOrderEventHandler
    (
        ILogger<CreateOrderEventHandler> logger,
        IPublishEndpoint publishEndpoint
    )
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task Handle(CreatedOrderEvent notification, CancellationToken cancellationToken)
    {
        const string functionName = $"{nameof(CreateOrderEventHandler)} =>";

        try
        {
            _logger.LogInformation($"{functionName} Handling Domain Event: {notification.GetType().Name}");
            var message = JsonConvert.SerializeObject(notification);
            _logger.LogInformation($"{functionName} Domain Event: {message}");
            
            // Publish to RabbitMQ
            var createOrderEvent = new CreateOrderEvent
            {
                UserId = notification.Order.UserId,
                TotalPrice = notification.Order.TotalPrice,
            };
            await _publishEndpoint.Publish<ICreateOrder>(new {Content = createOrderEvent}, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{functionName} Error: {ex.Message}");    
        }
    }
}