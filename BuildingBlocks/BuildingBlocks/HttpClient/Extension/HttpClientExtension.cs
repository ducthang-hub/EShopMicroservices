using BuildingBlocks.HttpClient.Implement;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.HttpClient.Extension;

public static class HttpClientExtension
{
    public static IServiceCollection AddTypedHttpClient<T>(this IServiceCollection services, T clientOption) where T : ServiceConfig
    {
        var clientName = typeof(T).Name;
        services.AddHttpClient<ICustomHttpClient<T>, CustomHttpClient<T>>(clientName, opts =>
        {
            opts.BaseAddress = new Uri(clientOption.BaseAddress);
        });

        return services;
    }
}