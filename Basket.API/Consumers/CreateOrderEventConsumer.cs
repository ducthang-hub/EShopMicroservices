using BuildingBlocks.MassTransit.Contracts.Queues;
using MassTransit;
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
