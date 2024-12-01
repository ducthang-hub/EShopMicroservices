using System.Net;
using Basket.API.Domains;
using Basket.API.DTOs;
using Basket.API.Persistence.Repositories;
using Mapster;
using MediatR;

namespace Basket.API.Features.Commands.ShoppingCartCommands.CreateShoppingCart;

public class CreateShoppingCartHandler
    (
        ILogger<CreateShoppingCartHandler> logger,
        IBasketRepository basketRepository
    )
    : IRequestHandler<CreateShoppingCartCommand, CreateShoppingCartResponse>
{

    public async Task<CreateShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        const string functionName = $"{nameof(CreateShoppingCartHandler)} =>";
        var response = new CreateShoppingCartResponse();
        
        try
        {
            var userId = Guid.NewGuid().ToString();
            var newCart = new ShoppingCart
            {
                UserId = userId,
            };
            newCart.PopulateAudit(userId);
            newCart.CartItems.AddRange(new ShoppingCartItem[]
            {
                new(Guid.NewGuid(), 2, userId)
            });
            await basketRepository.StoreBasketAsync(newCart, cancellationToken);
            var cartDto = newCart.Adapt<ShoppingCartDto>();

            response.Status = HttpStatusCode.OK;
            response.Data = cartDto;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{functionName} Error: {ex.Message}");        
        }

        return response;
    }
}