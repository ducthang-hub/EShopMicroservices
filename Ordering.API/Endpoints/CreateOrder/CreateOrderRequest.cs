using Ordering.Application.DTOs;
using Ordering.Domain.ValueObject;

namespace Ordering.API.Endpoints.CreateOrder;

public class CreateOrderRequest
{
    public string UserId { get; set; }
    public AddressDto ShippingAddress { get; set; }
    public PaymentDto Payment { get; set; }
    public IEnumerable<OrderItemDto> OrderItems { get; set; }
}