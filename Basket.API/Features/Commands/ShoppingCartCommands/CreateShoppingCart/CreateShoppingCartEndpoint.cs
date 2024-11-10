using Carter;
using MediatR;

namespace Basket.API.Features.Commands.ShoppingCartCommands.CreateShoppingCart;

public class CreateShoppingCartEndpoint : ICarterModule 
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket/create", async (IMediator mediator) =>
        {
            var response = await mediator.Send(new CreateShoppingCartCommand());
            return response;
        });
    }
}