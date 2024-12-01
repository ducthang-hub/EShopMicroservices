using Basket.API.Domains.Abstractions;
using BuildingBlocks.Contracts;

namespace Basket.API.Domains;

public class ShoppingCart : Aggregate<Guid>
{
    public List<ShoppingCartItem> CartItems { get; set; } = new();  
    public string UserId { get; set; } = string.Empty;
}