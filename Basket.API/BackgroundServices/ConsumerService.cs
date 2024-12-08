using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.Consumer;
using BuildingBlocks.MessageQueue.Requests;

namespace Basket.API.BackgroundServices;

public abstract class ConsumerService(IConsumer consumer, ILogger logger) : BackgroundService
{
    protected abstract ConsumeRequest ConsumeRequest { get; set; }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => ConsumeMessages(ConsumeRequest, cancellationToken), cancellationToken);
    }

    private async Task ConsumeMessages(ConsumeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await consumer.ConsumeMessages(request, HandleMessage, cancellationToken);
            cancellationToken.WaitHandle.WaitOne();
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }

    protected abstract Task HandleMessage(string message);
}