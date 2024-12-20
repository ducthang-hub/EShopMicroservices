using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Features.Commands.ShoppingCartCommands.CheckoutShoppingCart;

public class CheckoutShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/{cartId:guid}/checkout", async (IMediator mediator, Guid cartId, [FromBody] CheckoutShoppingCartPayload payload, CancellationToken cancellationToken) =>
        {
            payload.CartId = cartId;
            var response = await mediator.Send(new CheckoutShoppingCartCommand(payload), cancellationToken);
            return response;
        });
    }
}