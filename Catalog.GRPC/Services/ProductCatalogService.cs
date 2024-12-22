using BuildingBlocks.Helpers;
using Catalog.API.Persistence.DatabaseContext;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Catalog.GRPC.Services;

public class ProductCatalogService(IServiceProvider serviceProvider, ILogger<ProductCatalogService> logger) : ProductProtoService.ProductProtoServiceBase
{
    public override async Task<GetProductsResponse> GetProducts(GetProductsRequest request, ServerCallContext context)
    {
        var response = new GetProductsResponse();
        try
        {
            var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<CatalogDbContext>();
            var productIds = request.ProductIds;
            var products = await dbContext.Products
                .Where(i => productIds.Contains(i.Id.ToString()))
                .ToListAsync();
            foreach (var product in products)
            {
                response.Products.Add(product.Adapt<Product>());
            }
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
        return response;
    }
}