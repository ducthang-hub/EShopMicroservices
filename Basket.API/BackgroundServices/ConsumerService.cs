using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.Consumer;

namespace Basket.API.BackgroundServices;

public abstract class ConsumerService(IConsumer consumer, ILogger logger) : BackgroundService
{
    protected abstract string QueueName { get; set; }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => ConsumeMessages(cancellationToken), cancellationToken);
    }

    private async Task ConsumeMessages(CancellationToken cancellationToken)
    {
        try
        {
            await consumer.ConsumeMessages(QueueName, HandleMessage, cancellationToken);
            cancellationToken.WaitHandle.WaitOne();
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }

    protected abstract Task HandleMessage(string message);
}