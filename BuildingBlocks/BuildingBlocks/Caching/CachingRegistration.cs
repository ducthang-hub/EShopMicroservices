using BuildingBlocks.Caching.RedisCache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BuildingBlocks.Caching;

public static class CachingRegistration
{
    public static IServiceCollection AddCustomRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration["ConnectionStrings:Redis"];
        services
            .AddStackExchangeRedisCache(cfg => cfg.Configuration = configuration.GetConnectionString("Redis"))
            .AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connection!));
        
        services.AddScoped<IRedisCacheService, RedisCacheService>();
        
        return services;
    }
}