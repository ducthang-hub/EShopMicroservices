namespace BuildingBlock.Messaging.IntegrationEvents;

public record CreateOrderEvent : IntegrationEvent
{
    public string UserId { get; set; }
    public Guid CartId { get; set; }
    public double TotalPrice { get; set; }
    public IEnumerable<Guid> CartItemIds { get; set; }
}