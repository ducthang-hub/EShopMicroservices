using System.Text;
using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BuildingBlocks.MessageQueue.Consumer;

public class Consumer(IMessageQueueConnectionProvider connectionProvider, ILogger<Consumer> logger) : IConsumer
{
    public async Task ConsumeMessages(string exchange, Func<string, Task> handleMessage, CancellationToken cancellationToken)
    {
        try
        {
            var connection = await connectionProvider.GetConnection();
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.ExchangeDeclareAsync(exchange: exchange, ExchangeType.Fanout, cancellationToken: cancellationToken);

            var declareQueueResult = await channel.QueueDeclareAsync(cancellationToken: cancellationToken);
            var queueName = declareQueueResult.QueueName;
            await channel.QueueBindAsync(queue: queueName, exchange: exchange, routingKey: string.Empty, cancellationToken: cancellationToken);
            
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