using System.Text;
using BuildingBlocks.Helpers;
using BuildingBlocks.HttpClient.Extension;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.HttpClient.Implement;

public class CustomHttpClient<T>
    (
        ILogger<CustomHttpClient<T>> logger,
        IHttpClientFactory httpClientFactory,
        System.Net.Http.HttpClient httpClient
    ) : ICustomHttpClient<T> where T : ServiceConfig
{
    public async Task<TR?> GetAsync<TR>(string endpoint, CancellationToken cancellationToken) where TR : class
    {
        const string functionName = $"{nameof(CustomHttpClient<T>)} {nameof(GetAsync)} =>";

        try
        {
            logger.LogInformation($"{functionName} endpoint {endpoint}");
            using var httpClient = httpClientFactory.CreateClient(typeof(T).Name);
            var response = await httpClient.GetAsync(endpoint, cancellationToken);

            response.EnsureSuccessStatusCode();

            var responseAsString = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonHelper.Deserialize<TR>(responseAsString);
            return result;
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
            return default;
        }
    }

    public async Task<TR?> PostAsync<TRq, TR>(string endpoint, TRq request, CancellationToken cancellationToken) where TRq : class where TR : class
    {
        const string functionName = $"{nameof(CustomHttpClient<T>)} {nameof(PostAsync)} =>";
        try
        {
            logger.LogInformation($"{functionName} Endpoint = {endpoint}");
            var requestAsString = JsonHelper.Serialize(request);
            var requestContent = new StringContent(requestAsString, Encoding.UTF8, "application/json");

            using var httpClient = httpClientFactory.CreateClient(typeof(T).Name);
            var response = await httpClient.PostAsync(endpoint, requestContent, cancellationToken);

            response.EnsureSuccessStatusCode();
            var responseAsString = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonHelper.Deserialize<TR>(responseAsString);
            return result;
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
            return default;
        }
    }
}