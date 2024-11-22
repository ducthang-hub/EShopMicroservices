namespace BuildingBlocks.IntegrationEvents;

public class CreateOrderEvent
{
    public string UserId { get; set; }
    public double TotalPrice { get; set; }
}