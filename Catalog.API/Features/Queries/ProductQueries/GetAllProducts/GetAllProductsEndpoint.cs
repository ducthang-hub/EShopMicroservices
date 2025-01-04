using Carter;
using MediatR;

namespace Catalog.API.Features.Queries.ProductQueries.GetAllProducts;

public class GetAllProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (IMediator mediator, ILogger<GetAllProductsEndpoint> logger) =>
        {
            var response = new GetAllProductsResponse();
            try
            {
                var query = new GetAllProductsQuery();
                response = await mediator.Send(query);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error: {ex.Message}");
                response.Message = ex.Message;
            }
            return response;
        })
        .RequireAuthorization("student");;
    }
}