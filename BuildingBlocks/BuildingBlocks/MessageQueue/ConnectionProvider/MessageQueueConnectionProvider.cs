using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BuildingBlocks.MessageQueue.ConnectionProvider;

public class MessageQueueConnectionProvider : IMessageQueueConnectionProvider
{
    private readonly ILogger<MessageQueueConnectionProvider> _logger;
    private readonly IConnectionFactory _connectionFactory;
    private IConnection _connection;
    
    public MessageQueueConnectionProvider(ILogger<MessageQueueConnectionProvider> logger)
    {
        _connectionFactory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5673,
            UserName = "guest",
            Password = "guest"
        };

        _logger = logger;
    }
    
    public async Task<IConnection> GetConnection()
    {
        try
        {
            _connection = await _connectionFactory.CreateConnectionAsync();
            return _connection;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, $"Error: {ex.Message}");
            return default!;
        }
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}