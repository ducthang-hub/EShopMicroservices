using System.Net;
using BuildingBlocks.CQRS;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObject;
using Ordering.Infrastructure.Data.DatabaseContext;

namespace Ordering.Application.Features.Commands.CreateOrder;

public class CreateOrderHandler(OrderDbContext dbContext, ILogger<CreateOrderHandler> logger) : ICommandHandler<CreateOrderCommand, CreateOrderResponse>
{
    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateOrderResponse();
        try
        {
            var newOrder = CreateOrder(request);

            await dbContext.Orders.AddAsync(newOrder, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            response.Status = HttpStatusCode.Created;
            response.Data = newOrder;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(CreateOrderHandler)} Error: {ex.Message}");
            response.Message = ex.Message;
        }

        return response;
    }

    private Order CreateOrder(CreateOrderCommand request)
    {
        var userId = request.UserId;
        var addressDto = request.ShippingAddress;
        var paymentDto = request.Payment;
        var orderItems = request.OrderItems;

        var shippingAddress = Address.Of(addressDto.FirstName, addressDto.LastName, addressDto.EmailAddress,
            addressDto.AddressLine, addressDto.Country, addressDto.State, addressDto.ZipCode);
        var payment = Payment.Of(paymentDto.CardName, paymentDto.CardNumber, paymentDto.Expiration, paymentDto.Cvv,
            paymentDto.PaymentMethod);
        
        var newOrder = Order.Create(userId, shippingAddress, payment);
        foreach (var item in orderItems)
        {
            newOrder.AddOrderItem(item.ProductId, item.Quantity, item.Price);
        }

        return newOrder;
    }
}