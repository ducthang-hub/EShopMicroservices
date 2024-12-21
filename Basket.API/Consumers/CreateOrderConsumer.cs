using Basket.API.Persistence.Repositories;
using BuildingBlocks.Helpers;
using BuildingBlocks.MassTransit.Contracts.Queues;
using MassTransit;

namespace Basket.API.Consumers;

public class CreateOrderConsumer
(
    ILogger<CreateOrderConsumer> logger,
    IBasketRepository basketRepository
) : IConsumer<ICreateOrder>
{
    public async Task Consume(ConsumeContext<ICreateOrder> context)
    {
        const string functionName = $"{nameof(CreateOrderConsumer)} =>";
        try
        {
            var @event = context.Message.Content;
            var cart = await basketRepository.GetBasketAsync(@event.CartId, new CancellationToken());
            if (cart is null)
            {
                logger.LogError("{0} cart id {1} not found", functionName, @event.CartId);
                return;
            }

            cart.CartItems.Clear();
            await basketRepository.UpdateBasketAsync(cart, new CancellationToken());
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }

    public class CreateOrderConsumerDefinition : ConsumerDefinition<CreateOrderConsumer>
    {
        public CreateOrderConsumerDefinition()
        {
            EndpointName = "create-order";
            ConcurrentMessageLimit = 10;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<CreateOrderConsumer> consumerConfigurator,
            IRegistrationContext context)
        {
            
            // configure message retry with millisecond intervals
            endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

            // use the outbox to prevent duplicate events from being published
            endpointConfigurator.UseInMemoryOutbox(context);
        }
    }
}
