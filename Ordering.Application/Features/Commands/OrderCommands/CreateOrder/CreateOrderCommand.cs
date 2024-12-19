using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;
using Ordering.Application.DTOs;

namespace Ordering.Application.Features.Commands.OrderCommands.CreateOrder;

public class CreateOrderPayload
{
    public string UserId { get; set; }
    public Guid CartId { get; set; }
    public AddressDto ShippingAddress { get; set; }
    public PaymentDto Payment { get; set; }
    public IEnumerable<OrderItemDto> OrderItems { get; set; }
}
public class CreateOrderResponse : ErrorResponse{}

public class CreateOrderCommand(CreateOrderPayload payload) : ICommand<CreateOrderResponse>
{
    public CreateOrderPayload Payload { get; set; } = payload;
}