using System.Reflection;
using BuildingBlocks.PipelineBehaviors;
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

        return services;
    }
}