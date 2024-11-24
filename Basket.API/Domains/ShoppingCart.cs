using BuildingBlocks.Contracts;

namespace Basket.API.Domains;

public class ShoppingCart : AuditData
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;

    public IEnumerable<ShoppingCartItem> CartItems { get; set; }
}