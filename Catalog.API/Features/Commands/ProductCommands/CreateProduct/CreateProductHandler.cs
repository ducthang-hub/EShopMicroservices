using System.Net;
using BuildingBlocks.CQRS;
using Catalog.API.Domains;
using Catalog.API.Persistence.DatabaseContext;
using Mapster;

namespace Catalog.API.Features.Commands.ProductCommands.CreateProduct;


public class CreateProductHandler
    (
        CatalogDbContext dbContext
    )
    : ICommandHandler<CreateProductCommand, CreateProductResponse>
{
    public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateProductResponse();
        try
        {
            var product = request.Adapt<Product>();
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync(cancellationToken);
            response.Status = HttpStatusCode.Created;
            response.Data = new
            {
                ProductId = product.Id
            };
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}