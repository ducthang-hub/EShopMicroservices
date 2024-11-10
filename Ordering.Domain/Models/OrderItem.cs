using Ordering.Domain.Abstractions;

namespace Ordering.Domain.Models;

public class OrderItem : Entity<Guid>
{
    internal OrderItem(Guid orderId, Guid productId, uint quantity, double price)
    { 
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public uint Quantity { get; private set; }
    public double Price { get; private set; }
}