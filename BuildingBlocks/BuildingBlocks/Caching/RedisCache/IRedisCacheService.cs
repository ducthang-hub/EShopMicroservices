using Microsoft.Extensions.Caching.Distributed;

namespace BuildingBlocks.Caching.RedisCache;

public interface IRedisCacheService
{
    Task<T> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value);
    Task SetAsync<T>(string key, T value, TimeSpan absoluteExpirationRelativeToNow);
    Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions distributedCacheEntryOptions);
    Task RemoveAsync(string key);
    Task<T?> HashGetAsync<T>(string hashKey, string hashField);
    Task<bool> HashSetAsync(string hashKey, string hashField, object value, TimeSpan? expiration = null);
    public Task<bool> HashDeleteAsync(string hashKey, string hashField);

    public Task<long> HashDeleteAsync(string hashKey, string[] hashFields);
    Task HashSetAsync<T>(string hashKey, IDictionary<string, T> values, TimeSpan? expiration = null);
    Task<Dictionary<string, T>> HashGetAllAsync<T>(string hashKey);
}