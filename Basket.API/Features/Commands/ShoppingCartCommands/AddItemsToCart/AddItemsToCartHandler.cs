using System.Net;
using Basket.API.Domains;
using Basket.API.Persistence.Repositories;
using BuildingBlocks.CQRS;

namespace Basket.API.Features.Commands.ShoppingCartCommands.AddItemsToCart;

public class AddItemsToCartHandler
    (
        IBasketRepository basketRepository,
        ILogger<AddItemsToCartHandler> logger
    )
    : ICommandHandler<AddItemsToCartCommand, AddItemsToCartResponse>
{
    public async Task<AddItemsToCartResponse> Handle(AddItemsToCartCommand request, CancellationToken cancellationToken)
    {
        var payload = request.Payload;
        var functionName = $"{nameof(AddItemsToCartHandler)} ShoppingCartId = {payload.ShoppingCartId} =>";
        var response = new AddItemsToCartResponse();
        
        try
        {
            var items = new ShoppingCartItem[]
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductId = payload.ProductId,
                    Quantity = payload.Quantity
                }
            };
            var cartItems = await basketRepository.AddItemsToBasket(payload.ShoppingCartId, items, cancellationToken);

            if (cartItems.Any())
            {
                response.Status = HttpStatusCode.OK;
                response.Data = cartItems;
            }
            else
            {
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = "Cannot add items to cart";
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{functionName} Error: {ex.Message}");
        }

        return response;
    }
}