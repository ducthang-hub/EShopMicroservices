using Ordering.Domain.Abstractions;
using Ordering.Domain.Enums;
using Ordering.Domain.ValueObject;

namespace Ordering.Domain.Models;

public class Order : Aggregate<Guid>
{
    private List<OrderItem> _orderItems = new();
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.ToArray();
    public string UserId { get; set; }
    public Address ShippingAddress { get; set; }
    public Payment Payment { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Draft;
    public double TotalPrice => _orderItems.Sum(i => i.Price);

    public void AddOrderItem(Guid productId, uint quantity, double price)
    {
        var orderItem = new OrderItem(Id, productId, quantity, price);
        orderItem.PopulateAudit(this.UserId);
        _orderItems.Add(orderItem);
    }

    public OrderItem? RemoveOrderItem(Guid itemId)
    {
        var item = _orderItems.FirstOrDefault(i => i.Id == itemId);
        if (item != null) _orderItems.Remove(item);
        return item;
    }

    private Order(string userId, Address shippingAddress, Payment payment)
    {
        UserId = userId;
        ShippingAddress = shippingAddress;
        Payment = payment;
    }

    public static Order Create(string userId, Address shippingAddress, Payment payment)
    {
        var newOrder = new Order(userId, shippingAddress, payment);
        newOrder.PopulateAudit(userId);
        return newOrder;
    }
    
    
}


