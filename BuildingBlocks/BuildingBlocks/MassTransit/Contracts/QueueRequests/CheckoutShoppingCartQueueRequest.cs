using BuildingBlock.Messaging.IntegrationEvents;

namespace BuildingBlocks.MassTransit.Contracts.Queues;

public class CheckoutShoppingCartQueueRequest
{
    public ShoppingCartCheckoutEvent Content { get; set; }
}