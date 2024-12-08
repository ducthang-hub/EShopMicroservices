using System.Text;
using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using BuildingBlocks.MessageQueue.Requests;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BuildingBlocks.MessageQueue.Consumer;

public class Consumer(IMessageQueueConnectionProvider connectionProvider, ILogger<Consumer> logger) : IConsumer
{
    public async Task ConsumeMessages(ConsumeRequest request, Func<string, Task> handleMessage, CancellationToken cancellationToken)
    {
        try
        {
            var exchange = request.Exchange;
            var exchangeType = request.ExchangeType;
            var routingKeys = request.RoutingKeys;
            
            var connection = await connectionProvider.GetConnection();
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(exchange: exchange, exchangeType, cancellationToken: cancellationToken);

            var declareQueueResult = await channel.QueueDeclareAsync(cancellationToken: cancellationToken);
            var queueName = declareQueueResult.QueueName;
            foreach (var key in routingKeys)
            {
                var routingKey = key.ToLowerInvariant();
                await channel.QueueBindAsync(queue: queueName, exchange: exchange, routingKey: routingKey, cancellationToken: cancellationToken);
            }
            
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                handleMessage(message);
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queueName, autoAck: true, consumer: consumer, cancellationToken);
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }
}