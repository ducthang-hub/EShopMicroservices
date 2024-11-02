using Carter;
using MediatR;

namespace Basket.API.Features.Commands.BasketCommands.CreateBasket;

public class CreateBasketEndpoint(IMediator mediator) : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("basket/create", async (CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new CreateCartCommand(), cancellationToken);
            return response;
        });
    }
}