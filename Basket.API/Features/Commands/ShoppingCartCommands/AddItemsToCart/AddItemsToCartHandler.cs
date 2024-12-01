using System.Net;
using Basket.API.Domains;
using Basket.API.Persistence.Repositories;
using BuildingBlocks.CQRS;
using Mapster;

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
        var response = new AddItemsToCartResponse();
        
        try
        {
            var item = payload.Adapt<ShoppingCartItem>();
            item.PopulateAudit(payload.UserId);
            var addItemResult = await basketRepository.AddItemsToCart(item, cancellationToken);

            if (!addItemResult)
            {
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = "Cannot add item to cart";
            }
            else
            {
                response.Status = HttpStatusCode.OK;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error: {ex.Message}");
            response.Message = "Something went wrong";
        }

        return response;
    }
}