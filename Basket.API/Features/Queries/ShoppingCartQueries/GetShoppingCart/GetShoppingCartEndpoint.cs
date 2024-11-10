using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Features.Queries.ShoppingCartQueries.GetShoppingCart;

public class GetShoppingCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userId}", async (Guid userId, IMediator mediator) =>
        {
            var response = await mediator.Send(new GetShoppingCartQuery(userId));
            return response;
        });
    }
}