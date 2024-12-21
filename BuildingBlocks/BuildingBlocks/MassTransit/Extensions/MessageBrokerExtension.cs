using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.MassTransit.Extensions;

public static class MessageBrokerExtension
{
    public static IServiceCollection AddMessageBroker
    (
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly? assembly
    )
    {
        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();

            if (assembly != null)
            {
                cfg.AddConsumers(assembly);
                var consumerTypes = assembly.GetTypes()
                    .Where(t => typeof(IConsumer).IsAssignableFrom(t) && t is { IsAbstract: false, IsClass: true })
                    .ToList();
                Console.WriteLine($"Number of consumers registered: {consumerTypes.Count}");      
            }
            else
            {
                Console.Write("There is no consumer registered");
            }

            cfg.SetRabbitMqReplyToRequestClientFactory();
            cfg.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]);
                    host.Password(configuration["MessageBroker:Password"]);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}