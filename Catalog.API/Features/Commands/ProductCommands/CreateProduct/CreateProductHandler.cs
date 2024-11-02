using System.Net;
using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;
using BuildingBlocks.Services.Test;
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

public class CreateProductHandler : ICommandHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly ITest _test;
    private readonly CatalogDbContext _dbContext;
    
    public CreateProductHandler(CatalogDbContext dbContext, ITest testService)
    {
        _dbContext = dbContext;
        _test = testService;
    }
    public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateProductResponse();
        try
        {
            var product = request.Adapt<Product>();
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync(cancellationToken);
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