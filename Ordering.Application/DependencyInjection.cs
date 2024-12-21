using System.Reflection;
using BuildingBlocks.MassTransit.Contracts.Queues;
using BuildingBlocks.MassTransit.Extensions;
using BuildingBlocks.PipelineBehaviors;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        return services;
    }
}