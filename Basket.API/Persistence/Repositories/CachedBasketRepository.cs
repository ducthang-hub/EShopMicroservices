using Basket.API.Domains;
using BuildingBlocks.Caching.RedisCache;
namespace Basket.API.Persistence.Repositories;

public class CachedBasketRepository
    (
        IRedisCacheService redisCache,
        IBasketRepository repository,
        ILogger<CachedBasketRepository> logger
    ) 
    : IBasketRepository
{
    private const string HashKey = "Basket.ShoppingCart";
    
    public async Task<ShoppingCart?> GetBasketAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var cachedCart = await redisCache.HashGetAsync<ShoppingCart>(HashKey, id.ToString());
            if (cachedCart is not null) return cachedCart;
            var cart = await repository.GetBasketAsync(id, cancellationToken);
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
            await repository.StoreBasketAsync(cart, cancellationToken);
            await redisCache.HashSetAsync(HashKey, cart.Id.ToString(), cart);
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
            await redisCache.HashDeleteAsync(HashKey, id.ToString());
            var cart = await repository.DeleteBasketAsync(id, cancellationToken);
            return cart;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(CachedBasketRepository)} {nameof(DeleteBasketAsync)} Error: {ex.Message}");
            return null;
        }
    }
    
    public async Task<bool> UpdateBasketAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        try
        {
            await redisCache.HashSetAsync(HashKey, cart.Id.ToString(), cart);
            await repository.UpdateBasketAsync(cart, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AddItemsToCart(ShoppingCartItem item, CancellationToken cancellationToken)
    {
        try
        {
            var addItemResult = await repository.AddItemsToCart(item, cancellationToken);
            if (!addItemResult) return false;
            
            var cachedCart = await redisCache.HashGetAsync<ShoppingCart>(HashKey, item.ShoppingCartId.ToString());
            if (cachedCart is null)
            {
                cachedCart = await repository.GetBasketAsync(item.ShoppingCartId, cancellationToken);
            }
            else
            {
                cachedCart!.CartItems.Add(item);
            }
            
            await redisCache.HashSetAsync(HashKey, item.ShoppingCartId.ToString(), cachedCart!);
            return addItemResult;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error: {ex.Message}");
            return false;
        }
    }
}