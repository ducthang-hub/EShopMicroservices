using Basket.API.Domains.Abstractions;

namespace Basket.API.Domains;

public class ShoppingCartItem : Entity<Guid>
{
    public ShoppingCartItem()
    {
        
    }
    public ShoppingCartItem(Guid productId, uint quantity, string byUser)
    {
        ProductId = productId;
        Quantity = quantity;
        PopulateAudit(byUser);
    }
    public Guid ShoppingCartId { get; set; }
    public Guid ProductId { get; set; }
    public uint Quantity { get; set; }

    public ShoppingCart ShoppingCart { get; set; }
}