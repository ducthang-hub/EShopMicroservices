using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.CQRS.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddMediatR(this IServiceCollection services, Assembly assembly, Type[]? types = null)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            if (types is not null)
            {
                foreach (var type in types)
                {
                    cfg.AddOpenBehavior(type);
                }
            }
        });

        return services;
    }
    
}