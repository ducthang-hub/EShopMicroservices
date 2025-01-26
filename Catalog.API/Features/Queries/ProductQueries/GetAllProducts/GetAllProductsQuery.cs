using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;
using Catalog.API.Domains;

namespace Catalog.API.Features.Queries.ProductQueries.GetAllProducts;

public class GetAllProductsResponse : ErrorResponse
{
    public IEnumerable<Product> Products { get; set; }
}

public class GetAllProductsQuery : IQuery<GetAllProductsResponse>
{
}