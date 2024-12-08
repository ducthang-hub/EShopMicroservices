using RabbitMQ.Client;

namespace BuildingBlocks.MessageQueue.Consumer;

public interface IConsumer
{
    Task ConsumeMessages(string queue, Func<string, Task> handleMessage, CancellationToken cancellationToken);
}