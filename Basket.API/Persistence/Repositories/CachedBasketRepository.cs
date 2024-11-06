using Basket.API.Domains;
using Basket.API.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Basket.API.Persistence.Repositories;

public class CachedBasketRepository
    (
        IConnectionMultiplexer multiplexer,
        BasketDbContext dbContext,
        ILogger<CachedBasketRepository> logger
    ) 
    : IBasketRepository
{
    private readonly string HaskKey = "Basket.ShoppingCart";
    private readonly IDatabase _redis = multiplexer.GetDatabase();
    private readonly DbSet<ShoppingCart> _dbSet = dbContext.ShoppingCarts;
    
    public async Task<ShoppingCart?> GetBasketAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            ShoppingCart? cart;
            var cachedCarts = await _redis.HashGetAllAsync(HaskKey);
            if (cachedCarts.Length == 0)
            {
                cart = await _dbSet.Where(i => i.Id == id).FirstOrDefaultAsync(cancellationToken);
                return cart;
            }

            cart = cachedCarts.Select(i => JsonConvert.DeserializeObject<ShoppingCart>(i.Value!)).FirstOrDefault();
            return cart;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(GetBasketAsync)} Error: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        try
        {
            await _redis.HashSetAsync(HaskKey, cart.Id.ToString(), JsonConvert.SerializeObject(cart));
            _dbSet.Add(cart);
            dbContext.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(StoreBasketAsync)} Error: {ex.Message}");
            return false;
        }
    }

    public Task<ShoppingCart?> DeleteBasketAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}