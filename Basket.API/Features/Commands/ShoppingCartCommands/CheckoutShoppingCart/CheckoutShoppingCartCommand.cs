using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;

namespace Basket.API.Features.Commands.ShoppingCartCommands.CheckoutShoppingCart;

public class CheckoutShoppingCartPayload
{
    public Guid CartId { get; set; }

    public string UserId { get; set; }
    // Shipping and BillingAddress
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string AddressLine { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string State { get; set; } = default!;
    public string ZipCode { get; set; } = default!;

    // Payment
    public string CardName { get; set; } = default!;
    public string CardNumber { get; set; } = default!;
    public string Expiration { get; set; } = default!;
    public string CVV { get; set; } = default!;
    public int PaymentMethod { get; set; } = default!;
}

public class CheckoutShoppingCartResponse : ErrorResponse
{
}

public class CheckoutShoppingCartCommand(CheckoutShoppingCartPayload payload) : ICommand<CheckoutShoppingCartResponse>
{
    public CheckoutShoppingCartPayload Payload { get; set; } = payload;
}