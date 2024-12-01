using System.Linq.Expressions;
using Basket.API.Domains;

namespace Basket.API.Persistence.Repositories;

public interface IBasketRepository
{
    public Task<ShoppingCart?> GetBasketAsync(Guid id,  CancellationToken cancellationToken);
    public Task<bool> StoreBasketAsync(ShoppingCart cart,  CancellationToken cancellationToken);
    public Task<ShoppingCart?> DeleteBasketAsync(Guid id,  CancellationToken cancellationToken);
    public Task<bool> UpdateBasketAsync(ShoppingCart cart, CancellationToken cancellationToken);
    
    //todo: separate ShoppingCart repository and ShoppingCartItem repository
    public Task<bool> AddItemsToCart(ShoppingCartItem item, CancellationToken cancellationToken);
}