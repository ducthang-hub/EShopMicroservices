using Carter;
using MediatR;

namespace Basket.API.Features.Commands.ShoppingCartCommands.AddItemsToCart;

public class AddItemsToCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("basket/add-items",
            async (IMediator mediator, AddItemsToCartRequest request, CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(new AddItemsToCartCommand(request), cancellationToken);
                return response;
            });
    }
}