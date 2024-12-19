using System.Net;
using Basket.API.Persistence.Repositories;
using BuildingBlock.Messaging.IntegrationEvents;
using BuildingBlocks.CQRS;
using BuildingBlocks.Helpers;
using Mapster;
using MassTransit;

namespace Basket.API.Features.Commands.ShoppingCartCommands.CheckoutShoppingCart;

public class CheckoutShoppingCartHandler(
        IBasketRepository basketRepository,
        ILogger<CheckoutShoppingCartHandler> logger,
        IPublishEndpoint publishEndpoint
    ) 
    : ICommandHandler<CheckoutShoppingCartCommand, CheckoutShoppingCartResponse>
{
    public async Task<CheckoutShoppingCartResponse> Handle(CheckoutShoppingCartCommand request, CancellationToken cancellationToken)
    {
        var response = new CheckoutShoppingCartResponse();
        try
        {
            var payload = request.Payload; 
            var cart = await basketRepository.GetBasketAsync(payload.CartId, cancellationToken);
            if (cart is null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = $"Shopping cart id {payload.CartId} not found";
                return response;
            }

            var @event = payload.Adapt<ShoppingCartCheckoutEvent>(); 
            publishEndpoint.Publish(@event, cancellationToken);
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }

        return response;
    }
}