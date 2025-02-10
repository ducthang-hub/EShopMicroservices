using Catalog.API.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Catalog.API.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddDatabaseConnection(this IServiceCollection services,
        IConfiguration configuration)
    {
        var dataSource = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DatabaseConnection"))
            .EnableDynamicJson()
            .Build();

        services.AddDbContextPool<CatalogDbContext>((option) =>
        {
            option.UseNpgsql(dataSource);
        });

        return services;
    }
}