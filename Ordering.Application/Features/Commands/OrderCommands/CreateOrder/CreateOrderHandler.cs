using System.Net;
using BuildingBlocks.CQRS;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObject;
using Ordering.Infrastructure.Data.DatabaseContext;

namespace Ordering.Application.Features.Commands.OrderCommands.CreateOrder;

public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IOrderDbContext _orderDbContext;
    private readonly ILogger<CreateOrderHandler> _logger;

    public CreateOrderHandler
    (
        IOrderDbContext orderDbContext,
        ILogger<CreateOrderHandler> logger
    )
    {
        _orderDbContext = orderDbContext;
        _logger = logger;
    }
    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateOrderResponse();
        try
        {
            var newOrder = CreateOrder(request.Payload);

            await _orderDbContext.Orders.AddAsync(newOrder, cancellationToken);
            await _orderDbContext.SaveChangesAsync(cancellationToken);

            response.Status = HttpStatusCode.Created;
            response.Data = newOrder;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(CreateOrderHandler)} Error: {ex.Message}");
            response.Message = ex.Message;
        }

        return response;
    }

    private Order CreateOrder(CreateOrderPayload request)
    {
        var userId = request.UserId;
        var addressDto = request.ShippingAddress;
        var paymentDto = request.Payment;
        var orderItems = request.OrderItems;

        var shippingAddress = Address.Of(addressDto.FirstName, addressDto.LastName, addressDto.EmailAddress,
            addressDto.AddressLine, addressDto.Country, addressDto.State, addressDto.ZipCode);
        var payment = Payment.Of(paymentDto.CardName, paymentDto.CardNumber, paymentDto.Expiration, paymentDto.Cvv,
            paymentDto.PaymentMethod);
        
        var newOrder = Order.From(userId, shippingAddress, payment, OrderStatus.Pending);
        foreach (var item in orderItems)
        {
            newOrder.AddOrderItem(item.ProductId, item.Quantity, item.Price);
        }

        return newOrder;
    }
}