using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Ordering.Infrastructure.Data.DatabaseContext;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        // services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        // services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        var dataSource = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DatabaseConnection"))
            .EnableDynamicJson()
            .Build();

        services.AddDbContextPool<OrderDbContext>(opt => opt.UseNpgsql(dataSource));


        // services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}