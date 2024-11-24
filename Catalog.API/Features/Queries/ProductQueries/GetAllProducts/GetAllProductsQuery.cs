using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;

namespace Catalog.API.Features.Queries.ProductQueries.GetAllProducts;

public class GetAllProductsResponse : ErrorResponse
{
}

public class GetAllProductsQuery : IQuery<GetAllProductsResponse>
{
}