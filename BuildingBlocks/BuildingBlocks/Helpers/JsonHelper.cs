using Newtonsoft.Json;

namespace BuildingBlocks.Helpers;

public static class JsonHelper
{
    public static string Serialize(object o)
    {
        return JsonConvert.SerializeObject(o, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
    }
    
    public static T Deserialize<T>(string? json)
    {
        try
        {
            if (string.IsNullOrEmpty(json) || string.IsNullOrWhiteSpace(json))
            {
                return default!;
            }

            var value = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            })!;
            return value;
        }
        catch (Exception ex)
        {
            return default!;
        }
    }
}