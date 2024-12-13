using System.Text;
using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BuildingBlocks.Protocols.Rpc.RpcClient;

public class RpcClient<T>(IMessageQueueConnectionProvider connectionProvider, ILogger<RpcClient<T>> logger) : IRpcClient<T>
{
    private readonly TaskCompletionSource<T> _taskSource = new();
    private readonly Guid _correlationId = Guid.NewGuid();
    private string _replyQueue = string.Empty;
    
    public async Task<T> ProcessUnaryAsync(string queueName, CancellationToken cancellationToken)
    {
        try
        {
            await ConfigListeningReplyQueue(cancellationToken);
            await SendRequest(queueName, cancellationToken);
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
        
        return await _taskSource.Task;
    }
    
    private async Task ConfigListeningReplyQueue(CancellationToken cancellationToken)
    {
        try
        {
            var connection = await connectionProvider.GetConnection();
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
            
            var declareQueueResult = await channel.QueueDeclareAsync(cancellationToken: cancellationToken);
            _replyQueue = declareQueueResult.QueueName;
            
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (sender, @event) =>
            {
                var correlationId = @event.BasicProperties.CorrelationId;
                if (correlationId != _correlationId.ToString())
                {
                    logger.LogWarning("CorrelationId mismatch");
                    return Task.CompletedTask;
                }

                var body = @event.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var coupons = JsonHelper.Deserialize<T>(message);
                _taskSource.TrySetResult(coupons);
                return Task.CompletedTask;
            };
            
            await channel.BasicConsumeAsync(_replyQueue, true, consumer, cancellationToken);
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }
    
    private async Task SendRequest(string queueName, CancellationToken cancellationToken)
    {
        try
        {
            var connection = await connectionProvider.GetConnection();
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
            
            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);

            var basicProperties = new BasicProperties
            {
                CorrelationId = _correlationId.ToString(),
                ReplyTo = _replyQueue
            };
            
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, mandatory: true, basicProperties: basicProperties, body: new ReadOnlyMemory<byte>(), cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }
}