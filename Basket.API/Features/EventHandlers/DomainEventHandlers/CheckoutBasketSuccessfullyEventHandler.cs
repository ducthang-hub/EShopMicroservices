using Basket.API.Domains.Events;
using Basket.API.Persistence.Repositories;
using BuildingBlocks.Helpers;
using MediatR;

namespace Basket.API.Features.EventHandlers.DomainEventHandlers;

public class CheckoutBasketSuccessfullyEventHandler
(
    IBasketRepository basketRepository,
    ILogger<CheckoutBasketSuccessfullyEventHandler> logger
)
    : INotificationHandler<CheckoutBasketSuccessfullyEvent>
{
    public async Task Handle(CheckoutBasketSuccessfullyEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var cart = await basketRepository.GetBasketAsync(notification.CartId, cancellationToken);
            if (cart is null)
            {
                logger.LogError($"Cart id {notification.CartId} not found");
                return;
            }

            cart.CartItems.RemoveAll(i => notification.CheckoutProductIds.Contains(i.ProductId));
            await basketRepository.UpdateBasketAsync(cart, cancellationToken);
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }
}