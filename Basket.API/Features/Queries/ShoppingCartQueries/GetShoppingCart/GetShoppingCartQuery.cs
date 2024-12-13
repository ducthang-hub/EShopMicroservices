using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;

namespace Basket.API.Features.Queries.ShoppingCartQueries.GetShoppingCart;
public class GetShoppingCartResponse : ErrorResponse
{
}

public class GetShoppingCartQuery(Guid id) : IQuery<GetShoppingCartResponse>
{
    public Guid Id { get; set; } = id;
}