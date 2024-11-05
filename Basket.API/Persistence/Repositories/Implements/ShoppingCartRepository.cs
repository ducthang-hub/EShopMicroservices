using Basket.API.Domains;
using Basket.API.Persistence.DatabaseContext;
using Basket.API.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Persistence.Repositories.Implements;

public class ShoppingCartRepository : CachedRepository<ShoppingCart>, IShoppingCartRepository 
{
    public ShoppingCartRepository(BasketDbContext dbContext, IDistributedCache cache) : base(dbContext, cache)
    {
    }
}