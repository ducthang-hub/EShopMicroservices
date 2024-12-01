using System.Reflection;
using Basket.API.Persistence.DatabaseContext;
using Basket.API.Persistence.Repositories;
using BuildingBlocks.MassTransit.Extensions;
using Carter;
using Discount.GRPC;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Basket.API.Extensions;

public static class ServiceExtensions
{
    // public static IServiceCollection ConfigResiExchange(this IServiceCollection services, IConfiguration configuration)
    // {
    //     services
    //         .AddStackExchangeRedisCache(cfg => cfg.Configuration = configuration.GetConnectionString("Redis"))
    //         .AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6380"));
    //     
    //     services.AddScoped<IRedisCacheService, RedisCacheService>();
    //     
    //     return services;
    // }

    public static IServiceCollection ConfigMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });
        return services;
    }

    public static IServiceCollection ConfigDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<BasketDbContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("DatabaseConnection")));
        return services;
    }

    public static IServiceCollection ConfigGRPC(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcSettings:DiscountService"] ?? string.Empty);
        });
        return services;
    }

    public static IServiceCollection ConfigMessageBroker(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());
        return services;
    }

    public static IServiceCollection ConfigDomainRepository(this IServiceCollection services)
    {
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.Decorate<IBasketRepository, CachedBasketRepository>();

        return services;
    }

    public static IServiceCollection AddServicesInvocation(this IServiceCollection services)
    {
        services
            .AddCarter()
            .AddHttpClient()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        return services;
    }
}