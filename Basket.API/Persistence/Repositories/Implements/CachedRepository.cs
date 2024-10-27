using System.Linq.Expressions;
using Basket.API.Persistence.DatabaseContext;
using Basket.API.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Persistence.Repositories.Implements;

public class CachedRepository<T> : IRepository<T>
    where T : class
{
    private const string HashKey = "Basket.ShoppingCart";
    private readonly DbSet<T> _dbSet;
    private readonly IDistributedCache _cache;

    public CachedRepository
    (
        BasketDbContext dbContext,
        IDistributedCache cache
    )
    {
        _cache = cache;
        _dbSet = dbContext.Set<T>();
    }
    
    public IQueryable<T> GetAll()
    {
        var cachedEntities = _cache.GetString(HashKey);
        if (string.IsNullOrEmpty(cachedEntities)) return _dbSet;
        var entities = JsonConvert.DeserializeObject<List<T>>(cachedEntities);
        return entities.AsQueryable();
    }

    public async Task<bool> Add(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            var entityAsString = JsonConvert.SerializeObject(entity);
            await _cache.SetStringAsync(HashKey, entityAsString);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> AddRange(List<T> entities)
    {
        try
        {
            foreach (var entity in entities)
            {
                await Add(entity);
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool Delete(T entity)
    {
        throw new NotImplementedException();
    }

    public bool DeleteRange(List<T> entities)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRangeAsync(Expression<Func<T, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        throw new NotImplementedException();
    }


    public IQueryable<T> Where(Func<T, bool> func)
    {
        var cacheEntities = _cache.GetString(HashKey);
        if (!string.IsNullOrEmpty(cacheEntities))
        {
            var entities = JsonConvert.DeserializeObject<List<T>>(cacheEntities);
            return entities.Where(func).AsQueryable();
        }
        Expression<Func<T, bool>> expression = x => func(x);
        return _dbSet.Where(expression);
    }

    public EntityEntry<T> Update(T entity)
    {
        _cache.SetString(HashKey, JsonConvert.SerializeObject(entity));
        return _dbSet.Update(entity);
    }
}