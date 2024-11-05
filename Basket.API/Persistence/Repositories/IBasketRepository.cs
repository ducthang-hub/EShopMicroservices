using System.Linq.Expressions;
using Basket.API.Domains;

namespace Basket.API.Persistence.Repositories;

public interface IBasketRepository
{
    public Task<IEnumerable<ShoppingCart>> GetAll();
    public Task<IEnumerable<ShoppingCart>> Where();
    public Task AddAsync(IEnumerable<ShoppingCart> shoppingCarts);
    public Task DeleteAsync(Expression<Func<ShoppingCart, bool>> expression);
}