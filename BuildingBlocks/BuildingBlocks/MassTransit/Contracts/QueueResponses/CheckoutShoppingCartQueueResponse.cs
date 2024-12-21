namespace BuildingBlocks.MassTransit.Contracts.QueueResponses;

public class CheckoutShoppingCartQueueResponse
{
    public IEnumerable<Guid> InOrderProductIds { get; set; }
}