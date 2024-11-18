using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ordering.Domain.Events;

namespace Ordering.Application.Features.EventHandlers.DomainEvents;

public class CreateOrderEventHandler : INotificationHandler<CreatedOrderEvent>
{
    private readonly ILogger<CreateOrderEventHandler> _logger;

    public CreateOrderEventHandler
    (
        ILogger<CreateOrderEventHandler> logger
    )
    {
        _logger = logger;
    }
    
    public Task Handle(CreatedOrderEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{nameof(CreateOrderEventHandler)} Data = {JsonConvert.SerializeObject(notification.Order)}");
        return Task.CompletedTask;
    }
}