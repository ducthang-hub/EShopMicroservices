using System.Net;
using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using Catalog.API.Domains;
using Catalog.API.Persistent.DatabaseContext;
using Mapster;

namespace Catalog.API.Features.Commands.ProductCommands.CreateProduct;

public class CreateProductCommand(string name, double price) : ICommand<CreateProductResponse>
{
    public string Name { get; set; } = name;
    public double Price { get; set; } = price;
}

public class CreateProductResponse : ErrorResponse
{
}

public class CreateProductHandler(CatalogDbContext dbContext) : ICommandHandler<CreateProductCommand, CreateProductResponse>
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