using Carter;
using MediatR;

namespace Catalog.API.Features.Commands.ProductCommands.CreateProduct;

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductCommand product, IMediator mediator) =>
        {
            var response = await mediator.Send(product);
            return response;
        });
    }
}