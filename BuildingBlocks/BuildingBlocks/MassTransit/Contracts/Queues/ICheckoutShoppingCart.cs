using BuildingBlock.Messaging.IntegrationEvents;

namespace BuildingBlocks.MassTransit.Contracts.Queues;

public interface ICheckoutShoppingCart
{
    public ShoppingCartCheckoutEvent Content { get; set; }
}