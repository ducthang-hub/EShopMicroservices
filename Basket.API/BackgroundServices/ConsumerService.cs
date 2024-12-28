using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.Consumer;
using BuildingBlocks.MessageQueue.Requests;

namespace Basket.API.BackgroundServices;

public abstract class ConsumerService(IConsumer consumer, ILogger logger) : BackgroundService
{
    protected abstract ConsumeRequest ConsumeRequest { get; set; }
    protected abstract Task HandleMessage(string message);

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => ConsumeMessages(ConsumeRequest, cancellationToken), cancellationToken);
    }

    private async Task ConsumeMessages(ConsumeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            consumer.HandleMessage += HandleMessage;
            await consumer.ConsumeMessages(request, cancellationToken);
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }

}