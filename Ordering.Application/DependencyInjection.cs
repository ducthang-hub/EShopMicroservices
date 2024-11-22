using System.Reflection;
using BuildingBlocks.MassTransit.Contracts.Queues;
using BuildingBlocks.PipelineBehaviors;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration configuration, Assembly? entryAssembly = null)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddMassTransit(x =>
        {
            x.AddRequestClient<ICreateOrder>();
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("localhost", 5672,"/", h => {
                    h.Username("guest"); 
                    h.Password("guest");
                });
                    
                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}