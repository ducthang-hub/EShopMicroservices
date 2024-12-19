using BuildingBlock.Messaging.IntegrationEvents;
using BuildingBlocks.Helpers;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.DTOs;
using Ordering.Application.Features.Commands.OrderCommands.CreateOrder;

namespace Ordering.Application.Features.EventHandlers.IntegrationEvents;

public class BasketCheckoutEventConsumer
(
    ILogger<BasketCheckoutEventConsumer> logger,
    ISender sender
) : IConsumer<ShoppingCartCheckoutEvent>
{
    public async Task Consume(ConsumeContext<ShoppingCartCheckoutEvent> context)
    {
        try
        {
            var @event = context.Message;

            var address = new AddressDto(@event.FirstName, @event.LastName, @event.EmailAddress, @event.AddressLine,
                @event.Country, @event.State, @event.ZipCode);
            var payment = new PaymentDto(@event.CardName, @event.CardNumber, @event.Expiration, @event.CVV,
                @event.PaymentMethod);

            var createOrderPayload = new CreateOrderPayload
            {
                CartId = @event.CartId,
                ShippingAddress = address,
                Payment = payment,
                OrderItems = new[]
                {
                    new OrderItemDto()
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = 1,
                        Price = 11.5
                    }
                }
            };

            await sender.Send(new CreateOrderCommand(createOrderPayload));
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }
}