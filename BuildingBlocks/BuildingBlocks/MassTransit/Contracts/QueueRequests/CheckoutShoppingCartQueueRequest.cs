using BuildingBlock.Messaging.IntegrationEvents;

namespace BuildingBlocks.MassTransit.Contracts.QueueRequests;

public class CheckoutShoppingCartQueueRequest
{
    public ShoppingCartCheckoutEvent Content { get; set; }
}