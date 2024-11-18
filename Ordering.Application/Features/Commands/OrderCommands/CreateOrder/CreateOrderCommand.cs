using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;
using Ordering.Application.DTOs;

namespace Ordering.Application.Features.Commands.OrderCommands.CreateOrder;

public class CreateOrderResponse : ErrorResponse{}

public class CreateOrderCommand : ICommand<CreateOrderResponse>
{
    public string UserId { get; set; }
    public AddressDto ShippingAddress { get; set; }
    public PaymentDto Payment { get; set; }
    public IEnumerable<OrderItemDto> OrderItems { get; set; }
}