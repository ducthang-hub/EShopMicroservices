using System.Text;
using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using Discount.GRPC.Persistence.DatabaseContext;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Discount.GRPC.BackgroundServices;

public class CouponRpcServer(
    IMessageQueueConnectionProvider connectionProvider,
    ILogger<CouponRpcServer> logger,
    DiscountDbContext dbContext
) : BackgroundService
{
    private const string _queue = "rpc_coupon";
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var connection = await connectionProvider.GetConnection();
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(_queue, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);

            // todo: accept only one message per handling round
            
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var coupons = dbContext.Coupons.AsEnumerable();
                var couponsAsString = JsonHelper.Serialize(coupons);
                var body = Encoding.UTF8.GetBytes(couponsAsString);

                var replyQueue = ea.BasicProperties.ReplyTo;

                if (string.IsNullOrEmpty(replyQueue))
                {
                    logger.LogWarning("Cannot find reply queue");
                    return;
                }
                var replyProperties = new BasicProperties
                {
                    CorrelationId = ea.BasicProperties.CorrelationId
                };

                var senderConsumer = (AsyncEventingBasicConsumer)sender;
                var senderChannel = senderConsumer.Channel;
    
                await senderChannel.BasicPublishAsync(exchange: string.Empty, routingKey: replyQueue, mandatory: true,
                    basicProperties: replyProperties, body: body, cancellationToken: cancellationToken);
                
                //todo: acknowledge to client that server receives the message
            };

            await channel.BasicConsumeAsync(_queue, false, consumer, cancellationToken);
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }
}