using BuildingBlocks.MessageQueue.Requests;
using RabbitMQ.Client;

namespace BuildingBlocks.MessageQueue.Consumer;

public interface IConsumer
{
    Task ConsumeMessages(ConsumeRequest request, Func<string, Task> handleMessage, CancellationToken cancellationToken);
}