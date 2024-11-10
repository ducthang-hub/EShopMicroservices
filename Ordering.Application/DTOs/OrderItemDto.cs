namespace Ordering.Application.DTOs;

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public uint Quantity { get; set; }
    public double Price { get; set; }
}