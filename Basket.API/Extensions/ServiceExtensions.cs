using System.Reflection;
using Basket.API.BackgroundServices;
using Basket.API.BackgroundServices.BroadCast;
using Basket.API.BackgroundServices.Routing;
using Basket.API.BackgroundServices.WorkQueues;
using Basket.API.Domains;
using Basket.API.Persistence.DatabaseContext;
using Basket.API.Persistence.Repositories;
using BuildingBlocks.MassTransit.Extensions;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using BuildingBlocks.MessageQueue.Consumer;
using BuildingBlocks.MessageQueue.Producer;
using BuildingBlocks.Protocols.Rpc.RpcClient;
using Carter;
using Catalog.GRPC;
using Discount.GRPC;
using Microsoft.EntityFrameworkCore;

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
        services.AddGrpcClient<ProductProtoService.ProductProtoServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcSettings:CatalogService"] ?? string.Empty);
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

    public static IServiceCollection AddBackgroundService(this IServiceCollection services)
    {
        services.AddSingleton<IMessageQueueConnectionProvider, MessageQueueConnectionProvider>();
        services.AddSingleton<IConsumer, Consumer>();
        services.AddScoped<IProducer, Producer>();
        services.AddScoped<IRpcClient<IEnumerable<Coupon>>, RpcClient<IEnumerable<Coupon>>>();
        // services.AddHostedService<MessageConsumerService>();
        // services.AddHostedService<AnotherMessageConsumerService>();
        // services.AddHostedService<EmitLogConsumerService>();
        // services.AddHostedService<ReceiveLogConsumerService>();
        // services.AddHostedService<LogConsumerService>();
        // services.AddHostedService<LogErrorConsumerService>();
        services.AddSingleton<ConsumerService, EmitLogConsumerService>();
        services.AddSingleton<ConsumerService, ReceiveLogConsumerService>();
        services.AddSingleton<ConsumerService, AnotherMessageConsumerService>();
        services.AddSingleton<ConsumerService, MessageConsumerService>();
        services.AddSingleton<ConsumerService, LogConsumerService>();
        services.AddSingleton<ConsumerService, LogErrorConsumerService>();
        return services;
    }
    
    
}