using Basket.API.Domains;
using Basket.API.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Persistence.Repositories;

public class BasketRepository
    (
        BasketDbContext dbContext,
        ILogger<BasketRepository> logger
    )
    : IBasketRepository
{
    private readonly DbSet<ShoppingCartItem> _shoppingCartItem = dbContext.ShoppingCartItems;
    private readonly DbSet<ShoppingCart> _shoppingCart = dbContext.ShoppingCarts;


    public async Task<ShoppingCart?> GetBasketAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var cart = await _shoppingCart.Where(i => i.Id == id)
                .Include(i => i.CartItems)
                .FirstOrDefaultAsync(cancellationToken);
            return cart;
        }
        catch (Exception ex)
        {
            logger.LogError($"Error: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        try
        {
            await _shoppingCart.AddAsync(cart, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error: {ex.Message}");
            return false;
        }
    }

    public async Task<ShoppingCart?> DeleteBasketAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var cartToDelete = await GetBasketAsync(id, cancellationToken);
            if (cartToDelete is null) return null;

            _shoppingCart.Remove(cartToDelete);
            await dbContext.SaveChangesAsync(cancellationToken);
            return cartToDelete;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> UpdateBasketAsync(ShoppingCart cart, CancellationToken cancellationToken)
    {
        try
        {
            _shoppingCart.Update(cart);
            await dbContext.SaveChangesAsync(cancellationToken);
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
            await _shoppingCartItem.AddAsync(item, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error: {ex.Message}");
            return false;
        }
    }
}