using System.Net;
using BuildingBlocks.CQRS;
using Catalog.API.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.Queries.ProductQueries.GetAllProducts;

public class GetAllProductsHandler
    (
        CatalogDbContext dbContext
    )
    : IQueryHandler<GetAllProductsQuery, GetAllProductsResponse>
{
    public async Task<GetAllProductsResponse> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAllProductsResponse();
        try
        {
            var products = await dbContext.Products.ToListAsync(cancellationToken);
            response.Data = new
            {
                Products = products
            };
            response.Status = HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}