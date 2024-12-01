using BuildingBlocks.Caching.RedisCache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BuildingBlocks.Caching;

public static class CachingRegistration
{
    public static IServiceCollection AddCustomRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddStackExchangeRedisCache(cfg => cfg.Configuration = configuration.GetConnectionString("Redis"))
            .AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6380"));
        
        services.AddScoped<IRedisCacheService, RedisCacheService>();
        
        return services;
    }
}