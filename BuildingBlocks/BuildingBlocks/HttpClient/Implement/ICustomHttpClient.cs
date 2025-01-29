using BuildingBlocks.HttpClient.Extension;

namespace BuildingBlocks.HttpClient.Implement;

public interface ICustomHttpClient<T> where T : ServiceConfig
{
    Task<TR?> GetAsync<TR>(string endpoint, CancellationToken cancellationToken) where TR : class;
    Task<TR?> PostAsync<T, TR>(string endpoint, T request, CancellationToken cancellationToken) where T : class where TR : class;
}