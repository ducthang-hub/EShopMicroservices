using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BuildingBlocks.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigNewtonSoft(this IServiceCollection services)
    {
        services.AddControllers()
            .AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                }
            )
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });
        return services;
    } 
}