using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Extensions.Extensions;

public static class ConfigurationExtension
{
    public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
    {
        var model = new TModel();
        configuration.GetSection(section).Bind(model);
        return model;
    }
}