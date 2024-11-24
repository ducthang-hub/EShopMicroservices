using BuildingBlocks.Contracts;
using Microsoft.VisualBasic.CompilerServices;

namespace Basket.API.Domains;

public class ShoppingCartItem : AuditData
{
    public Guid Id { get; set; }
    public Guid ShoppingCartId { get; set; }
    public Guid ProductId { get; set; }
    public uint Quantity { get; set; }

    public ShoppingCart ShoppingCart { get; set; }
}