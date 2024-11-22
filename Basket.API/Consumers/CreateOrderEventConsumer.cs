using BuildingBlocks.MassTransit.Contracts.Queues;
using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using Newtonsoft.Json;

namespace Basket.API.Consumers;

public class CreateOrderEventConsumer(ILogger<CreateOrderEventConsumer> logger) : IConsumer<ICreateOrder>
{
    public Task Consume(ConsumeContext<ICreateOrder> context)
    {
        const string functionName = $"{nameof(CreateOrderEventConsumer)} =>";
        var order = context.Message.Content;
        logger.LogInformation($"{functionName} Event content: {JsonConvert.SerializeObject(order)}");
        return Task.CompletedTask;
    }
}


class CreateOrderEventConsumerDefinition : ConsumerDefinition<CreateOrderEventConsumer>
{
    public CreateOrderEventConsumerDefinition()
    {
    }

    protected override void ConfigureConsumer
    (
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<CreateOrderEventConsumer> consumerConfigurator
    )
    {
        endpointConfigurator.UseMessageRetry(r => r.Interval(1, TimeSpan.FromSeconds(60)));
    }
}