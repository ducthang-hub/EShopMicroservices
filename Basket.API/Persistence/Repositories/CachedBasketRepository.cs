using Basket.API.Domains;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Basket.API.Persistence.Repositories;

public class CachedBasketRepository
    (
        IConnectionMultiplexer multiplexer,
        IBasketRepository repository,
        ILogger<CachedBasketRepository> logger
    ) 
    : IBasketRepository
{
    private const string HaskKey = "Basket.ShoppingCart";
    private readonly IDatabase _redis = multiplexer.GetDatabase();
    
    public async Task<ShoppingCart?> GetBasketAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            ShoppingCart? cart;
            var cachedCarts = await _redis.HashGetAllAsync(HaskKey);
            if (cachedCarts.Length == 0)
            {
                cart = await repository.GetBasketAsync(id, cancellationToken);
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
            await repository.StoreBasketAsync(cart, cancellationToken);
            await _redis.HashSetAsync(HaskKey, cart.Id.ToString(), JsonConvert.SerializeObject(cart));
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
            await _redis.HashDeleteAsync(HaskKey, id.ToString());
            var cart = await repository.DeleteBasketAsync(id, cancellationToken);
            return cart;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{nameof(CachedBasketRepository)} {nameof(DeleteBasketAsync)} Error: {ex.Message}");
            return null;
        }
    }
}