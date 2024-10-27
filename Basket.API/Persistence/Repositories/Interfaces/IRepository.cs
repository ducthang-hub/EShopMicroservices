using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Basket.API.Persistence.Repositories.Interfaces;

public interface IRepository<T> where T : class 
{
    IQueryable<T> GetAll();
    Task<bool> Add(T entity);
    Task<bool> AddRange(List<T> entities);
    bool Delete(T entity);
    bool DeleteRange(List<T> entities);
    Task DeleteRangeAsync(Expression<Func<T, bool>> expression);
    IQueryable<T> Where(Expression<Func<T, bool>> expression);
    EntityEntry<T> Update(T entity);
}