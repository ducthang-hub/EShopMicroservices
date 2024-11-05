using System.Linq.Expressions;
using Basket.API.Domains;
using Basket.API.Persistence.DatabaseContext;
using Mapster;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Basket.API.Persistence.Repositories;

public class BasketRepository
    (
        BasketDbContext dbContext,
        IConnectionMultiplexer multiplexer
    )
    : IBasketRepository
{
    private const string HashKey = "Basket.ShoppingCart";
    private readonly DbSet<ShoppingCart> _dbSet = dbContext.ShoppingCarts;
    private readonly IDatabase _redis = multiplexer.GetDatabase();

    public async Task<IEnumerable<ShoppingCart>> GetAll()
    {
        var cachedCarts = await _redis.HashGetAllAsync(HashKey);
        if (cachedCarts.Length == 0)
        {
            return _dbSet;
        }

        var carts = cachedCarts.Select(i => i.Value).ToList();
        return carts.Adapt<List<ShoppingCart>>();
    }

    public Task<IEnumerable<ShoppingCart>> Where()
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(IEnumerable<ShoppingCart> shoppingCarts)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Expression<Func<ShoppingCart, bool>> expression)
    {
        throw new NotImplementedException();
    }
}