using BuildingBlocks.MessageQueue.Requests;
using RabbitMQ.Client;

namespace BuildingBlocks.MessageQueue.Consumer;

public interface IConsumer
{
    Consumer.HandleMessageFunc HandleMessage { get; set; }
    Task ConsumeMessages(ConsumeRequest request, CancellationToken cancellationToken);
}