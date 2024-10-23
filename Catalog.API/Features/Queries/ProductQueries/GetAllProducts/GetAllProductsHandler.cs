using System.Net;
using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;
using Catalog.API.Persistent.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Features.Queries.ProductQueries.GetAllProducts;

public class GetAllProductsResponse : ErrorResponse
{
}

public class GetAllProductsQuery : IQuery<GetAllProductsResponse>
{
}

public class GetAllProductsHandler : IQueryHandler<GetAllProductsQuery, GetAllProductsResponse>
{
    private readonly CatalogDbContext _dbContext;

    public GetAllProductsHandler
    (
        CatalogDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }
    
    public async Task<GetAllProductsResponse> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAllProductsResponse();
        try
        {
            var products = await _dbContext.Products.ToListAsync(cancellationToken);
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