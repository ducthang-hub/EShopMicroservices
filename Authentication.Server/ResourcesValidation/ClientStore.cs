using Authentication.Server.Persistence.DatabaseContext;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Server.ResourcesValidation;

public class ClientStore : IClientStore
{
    private readonly AuthDbContext _dbContext;

    public ClientStore(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Client> FindClientByIdAsync(string clientId)
    {
        try
        {
            var clientData = await (
                from cl in _dbContext.Client
                join sp in _dbContext.ClientScope.Include(i => i.ApiScope)
                    on cl.Id equals sp.ClientId
                join sc in _dbContext.ClientSecret
                    on cl.Id equals sc.ClientId
                join gt in _dbContext.ClientGrantType
                    on cl.Id equals gt.ClientId
                select new
                {
                    Client = cl,
                    ApiScope = sp,
                    ClientSecret = sc,
                    ClientGrantType = gt
                }
            )
            .ToListAsync();

            if(clientData.Count == 0)
            {
                return new Client();
            }

            var client = clientData.Select(i => i.Client).FirstOrDefault();
            var apiScope = clientData.Select(i => i.ApiScope).Distinct().ToList();
            var clientSecret = clientData.Select(i => i.ClientSecret).Distinct().ToList();  
            var grantType = clientData.Select(i => i.ClientGrantType).Distinct().ToList();

            var returnClient = new Client
            {
                ClientId = client?.ClientId ?? clientId,
                AllowedScopes = apiScope?.Select(i => i.ApiScope.Name).ToList() ?? [],
                ClientSecrets = clientSecret?.Select(i => new Secret(i.Secret.Sha256())).ToList() ?? null,
                AllowedGrantTypes = grantType?.Select(i => i.GrantType).ToList() ?? [],
                AllowOfflineAccess = true,
                AbsoluteRefreshTokenLifetime = 345600,
                AccessTokenLifetime = 14400,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                RefreshTokenUsage = TokenUsage.ReUse,
            };

            return returnClient;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Client();
        }
    }

}