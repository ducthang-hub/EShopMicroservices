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
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        // services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();
        var dataSource = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DatabaseConnection"))
            .EnableDynamicJson()
            .Build();

        services.AddDbContextPool<OrderDbContext>((serviceProvider, option) =>
        {
            option.UseNpgsql(dataSource);
            option.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
        });


        // services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}