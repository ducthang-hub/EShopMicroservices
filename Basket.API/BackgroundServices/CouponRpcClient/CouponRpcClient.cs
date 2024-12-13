using System.Text;
using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using Discount.GRPC;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Basket.API.BackgroundServices.CouponRpcClient;

public class CouponRpcClient(IMessageQueueConnectionProvider connectionProvider, ILogger<CouponRpcClient> logger) : ICouponRpcClient
{
    private const string _queue = "rpc_coupon";
    private readonly Guid _correlationId = Guid.NewGuid();
    private string _replyQueue;
    private readonly TaskCompletionSource<IEnumerable<CouponModel>> _coupons = new();
    
    public async Task<IEnumerable<CouponModel>> GetCouponsAsync(CancellationToken cancellationToken)
    {
        try
        {
            await ConfigListeningReplyQueue(cancellationToken);
            await SendRequest(cancellationToken);
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
        return await _coupons.Task;
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

                var coupons = JsonHelper.Deserialize<IEnumerable<CouponModel>>(message);
                _coupons.TrySetResult(coupons);
                return Task.CompletedTask;
            };
            
            await channel.BasicConsumeAsync(_replyQueue, true, consumer, cancellationToken);
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }
    
    private async Task SendRequest(CancellationToken cancellationToken)
    {
        try
        {
            var connection = await connectionProvider.GetConnection();
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
            
            await channel.QueueDeclareAsync(_queue, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);

            var basicProperties = new BasicProperties
            {
                CorrelationId = _correlationId.ToString(),
                ReplyTo = _replyQueue
            };
            
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: _queue, mandatory: true, basicProperties: basicProperties, body: new ReadOnlyMemory<byte>(), cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }
}