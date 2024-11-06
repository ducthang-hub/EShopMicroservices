using Basket.API.Domains;
using Basket.API.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Basket.API.Persistence.Repositories;

public class BasketRepository
    (
        BasketDbContext dbContext,
        ILogger<BasketRepository> logger
    )
    : IBasketRepository
{
    private readonly DbSet<ShoppingCart> _dbSet = dbContext.ShoppingCarts;


    public async Task<ShoppingCart?> GetBasketAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var cart = await _dbSet.Where(i => i.Id == id).FirstOrDefaultAsync(cancellationToken);
            return cart;
        }
        catch (Exception ex)
        {
            logger.LogError($"{nameof(GetBasketAsync)} Error: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        try
        {
            await _dbSet.AddAsync(cart, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(StoreBasketAsync)} Error: {ex.Message}");
            return false;
        }
    }

    public async Task<ShoppingCart?> DeleteBasketAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var cartToDelete = await GetBasketAsync(id, cancellationToken);
            if (cartToDelete is null) return null;

            _dbSet.Remove(cartToDelete);
            return cartToDelete;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(StoreBasketAsync)} Error: {ex.Message}");
            return null;
        }
    }
}