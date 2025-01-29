using BuildingBlocks.Extensions.Extensions;
using BuildingBlocks.HttpClient.Extension;
using BuildingBlocks.HttpClient.Implement;

namespace Catalog.API.Extensions;

public static class CustomHttpClient
{
    public static IServiceCollection AddCustomHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceOptions = configuration.GetOptions<ServiceOptions>(ServiceOptions.OptionName);

        services.AddTypedHttpClient(serviceOptions.AuthenticationService);

        services.AddScoped(typeof(ICustomHttpClient<>), typeof(CustomHttpClient<>));
        return services;
    }
}