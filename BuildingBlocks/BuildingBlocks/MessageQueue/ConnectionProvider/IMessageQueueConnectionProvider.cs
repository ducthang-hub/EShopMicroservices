using RabbitMQ.Client;

namespace BuildingBlocks.MessageQueue.ConnectionProvider;

public interface IMessageQueueConnectionProvider : IDisposable
{
    Task<IConnection> GetConnection();
}