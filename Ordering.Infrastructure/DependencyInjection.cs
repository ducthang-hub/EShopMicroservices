using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Ordering.Infrastructure.Data.DatabaseContext;
using Ordering.Infrastructure.Interceptors;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();
        var dataSource = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DatabaseConnection"))
            .EnableDynamicJson()
            .Build();

        services.AddDbContext<OrderDbContext>((serviceProvider, option) =>
        {
            option.UseNpgsql(dataSource);
            option.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
        });

        services.AddScoped<IOrderDbContext, OrderDbContext>();
        
        return services;
    }
}