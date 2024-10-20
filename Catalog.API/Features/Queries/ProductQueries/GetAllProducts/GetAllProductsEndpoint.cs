using Carter;
using MediatR;

namespace Catalog.API.Features.Queries.ProductQueries.GetAllProducts;

public class GetAllProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (IMediator mediator) =>
        {
            var query = new GetAllProductsQuery();
            var response = await mediator.Send(query);
            return response;
        });
    }
}