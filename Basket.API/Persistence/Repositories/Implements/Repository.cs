using System.Linq.Expressions;
using Basket.API.Persistence.DatabaseContext;
using Basket.API.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Basket.API.Persistence.Repositories.Implements;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly BasketDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public Repository(BasketDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public virtual IQueryable<T> GetAll()
    {
        return _dbSet;
    }

    public virtual async Task<bool> Add(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public virtual async Task<bool> AddRange(List<T> entities)
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

    public virtual bool Delete(T entity)
    {
        try
        {
            _dbSet.Remove(entity);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public virtual bool DeleteRange(List<T> entities)
    {
        try
        {
            _dbSet.RemoveRange(entities);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public virtual async Task DeleteRangeAsync(Expression<Func<T, bool>> expression)
    {
        var entities = await _dbSet.Where(expression).ToListAsync();
        DeleteRange(entities);
    }

    public virtual IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression);
    }

    public virtual EntityEntry<T> Update(T entity)
    {
        return _dbSet.Update(entity);
    }
}