using Basket.API.Domains.Events;
using Basket.API.Persistence.Repositories;
using BuildingBlocks.Helpers;
using MediatR;

namespace Basket.API.Features.EventHandlers.DomainEventHandlers;

public class CheckoutBasketSuccessfullyEventHandler
(
    ILogger<CheckoutBasketSuccessfullyEventHandler> logger,
    IServiceProvider serviceProvider
)
    : INotificationHandler<CheckoutBasketSuccessfullyEvent>
{
    public async Task Handle(CheckoutBasketSuccessfullyEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var scope = serviceProvider.CreateScope();
            var basketRepository = scope.ServiceProvider.GetService<IBasketRepository>();
            if (basketRepository is null)
            {
                logger.LogError("Can not get basket repository service to delete basket item");
                return;
            }
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