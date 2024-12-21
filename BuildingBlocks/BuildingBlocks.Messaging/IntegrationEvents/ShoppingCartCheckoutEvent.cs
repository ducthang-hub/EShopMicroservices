namespace BuildingBlock.Messaging.IntegrationEvents;

public record ShoppingCartCheckoutEvent : IntegrationEvent
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
    public IEnumerable<CartItemDto> CartItems { get; set; }
}

public record CartItemDto
{
    public Guid ShoppingCartId { get; set; }
    public Guid ProductId { get; set; }
    public uint Quantity { get; set; }
    //todo: create grpc service to get price from product service
    public double Price { get; set; } = 10.5d;
}