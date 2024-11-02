using Carter;
using MediatR;

namespace Basket.API.Features.Queries.BasketQueries.GetBasket;

public class GetBasketEndpoint(IMediator mediator) : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userId}", async (CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new GetBasketQuery(), cancellationToken);
            return response;
        });
    }
}