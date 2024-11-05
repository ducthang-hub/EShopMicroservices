using System.Linq.Expressions;
using Basket.API.Persistence.DatabaseContext;
using Basket.API.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Persistence.Repositories.Implements;

public class CachedRepository<T>
    (
        BasketDbContext dbContext,
        IDistributedCache cache
    ) 
    : IRepository<T>where T : class
{
    private const string HashKey = "Basket.ShoppingCart";
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public IQueryable<T> GetAll()
    {
        var cachedEntities = cache.GetString(HashKey);
        if (string.IsNullOrEmpty(cachedEntities)) return _dbSet;
        var entities = JsonConvert.DeserializeObject<List<T>>(cachedEntities);
        return entities.AsQueryable();
    }

    public async Task<bool> Add(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            // var entityAsString = JsonConvert.SerializeObject(entity);
            // await _cache.SetStringAsync(HashKey, entityAsString);
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
        var cacheEntities = cache.GetString(HashKey);
        if (string.IsNullOrEmpty(cacheEntities)) return _dbSet.Where(expression);
        
        var entities = JsonConvert.DeserializeObject<T>(cacheEntities);
        if (entities is null)
        {
            return null;
        }
        var list = new List<T> { entities };
        var func = expression.Compile();
        return list.Where(func).AsQueryable();
    }

    public EntityEntry<T> Update(T entity)
    {
        cache.SetString(HashKey, JsonConvert.SerializeObject(entity));
        return _dbSet.Update(entity);
    }
}