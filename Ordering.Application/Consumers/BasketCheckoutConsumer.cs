using System.Net;
using BuildingBlock.Messaging.IntegrationEvents;
using BuildingBlocks.Helpers;
using BuildingBlocks.MassTransit.Contracts.QueueRequests;
using BuildingBlocks.MassTransit.Contracts.QueueResponses;
using BuildingBlocks.MassTransit.Contracts.Queues;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.DTOs;
using Ordering.Application.Features.Commands.OrderCommands.CreateOrder;
using Ordering.Domain.Models;

namespace Ordering.Application.Consumers;

public class BasketCheckoutConsumer : IConsumer<CheckoutShoppingCartQueueRequest>
{
    private readonly ILogger<BasketCheckoutConsumer> _logger;
    private readonly ISender _sender;
    public BasketCheckoutConsumer(
        ILogger<BasketCheckoutConsumer> logger,
        ISender sender
    )
    {
        _logger = logger;
        _sender = sender;
    }
    public async Task Consume(ConsumeContext<CheckoutShoppingCartQueueRequest> context)
    {
        try
        {
            var @event = context.Message.Content;

            var address = new AddressDto(@event.FirstName, @event.LastName, @event.EmailAddress, @event.AddressLine,
                @event.Country, @event.State, @event.ZipCode);
            var payment = new PaymentDto(@event.CardName, @event.CardNumber, @event.Expiration, @event.CVV,
                @event.PaymentMethod);

            var createOrderPayload = new CreateOrderPayload
            {
                UserId = @event.UserId,
                CartId = @event.CartId,
                ShippingAddress = address,
                Payment = payment,
                OrderItems = @event.CartItems.Select(i => i.Adapt<OrderItemDto>())
            };

            var createOrderResponse = await _sender.Send(new CreateOrderCommand(createOrderPayload));
            if (createOrderResponse.Status != HttpStatusCode.Created)
            {
                // do something that I dont know yet
                return;
            }
            
            var queueResponse = new CheckoutShoppingCartQueueResponse
            {
                InOrderProductIds = createOrderResponse.ProductIds
            };
            await context.RespondAsync(queueResponse);
        }
        catch (Exception ex)
        {
            ex.LogError(_logger);
        }
    }
}

internal class BasketCheckoutConsumerDefinition : ConsumerDefinition<BasketCheckoutConsumer>
{
    public BasketCheckoutConsumerDefinition()
    {
        // override the default endpoint name
        EndpointName = "basket-checkout";

        // limit the number of messages consumed concurrently
        // this applies to the consumer only, not the endpoint
        ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<BasketCheckoutConsumer> consumerConfigurator,
        IRegistrationContext context
    )
    {
        // configure message retry with millisecond intervals
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));

        // use the outbox to prevent duplicate events from being published
        endpointConfigurator.UseInMemoryOutbox(context);
    }
}
