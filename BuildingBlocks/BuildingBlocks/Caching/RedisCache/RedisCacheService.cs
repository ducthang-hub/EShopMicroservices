using System.Runtime.CompilerServices;
using BuildingBlocks.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace BuildingBlocks.Caching.RedisCache;

public class RedisCacheService(IConnectionMultiplexer multiplexer) : IRedisCacheService
{
    private readonly IDatabase _database = multiplexer.GetDatabase();
    
    public Task<T> GetAsync<T>(string key)
    {
        throw new NotImplementedException();
    }

    public Task SetAsync<T>(string key, T value)
    {
        throw new NotImplementedException();
    }

    public Task SetAsync<T>(string key, T value, TimeSpan absoluteExpirationRelativeToNow)
    {
        throw new NotImplementedException();
    }

    public Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions distributedCacheEntryOptions)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string key)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> HashGetAsync<T>(string hashKey, string hashField)
    {
        var redisValueString = await _database.HashGetAsync(hashKey, hashField.ToLower());
        var redisValue = JsonHelper.Deserialize<T>(redisValueString);
        return redisValue;
    }

    public async Task<bool> HashSetAsync(string hashKey, string hashField, object value, TimeSpan? expiration = null)
    {
        try
        {
            var serializedValue = JsonHelper.Serialize(value);
            var isSuccess = await _database.HashSetAsync(hashKey, hashField.ToLower(), serializedValue);
            if (expiration != null)
            {
                _database.KeyExpire(hashKey, expiration);
            }

            return isSuccess;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    
    public async Task<bool> HashDeleteAsync(string hashKey, string hashField)
    {
        return await _database.HashDeleteAsync(hashKey, hashField.ToLower());
    }

    public async Task<long> HashDeleteAsync(string hashKey, string[] hashFields)
    {
        return await _database.HashDeleteAsync(hashKey, hashFields.Select(x => (RedisValue)x.ToLower()).ToArray());
    }

    public Task HashSetAsync<T>(string hashKey, IDictionary<string, T> values, TimeSpan? expiration = null)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, T>> HashGetAllAsync<T>(string hashKey)
    {
        throw new NotImplementedException();
    }
}