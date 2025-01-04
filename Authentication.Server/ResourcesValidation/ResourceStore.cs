using Authentication.Server.Persistence.DatabaseContext;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Server.ResourcesValidation;

public class ResourceStore(AuthDbContext dbContext) : IResourceStore
{
    public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
    {
        try
        {
            var apiResourceData = await dbContext.ApiResource.Where(i => apiResourceNames.Contains(i.Name)).ToListAsync();
            if (!apiResourceData.Any())
            {
                return new List<ApiResource>();
            }

            var apiResource = apiResourceData.Select(i => new ApiResource {
                Name = i.Name,
                ApiSecrets = new List<Secret> { new Secret(i.Secret.ToString().Sha256()) }
            }).ToList();

            return apiResource;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<ApiResource>(); 
        }
    }

    public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        try
        {
            var apiResourceData = await (
                from ar in dbContext.ApiResource
                join sr in dbContext.ApiScopeResource
                    on ar.Id equals sr.ApiResourceId
                join sp in dbContext.ApiScope
                    on sr.ApiScopeId equals sp.Id
                where scopeNames.Contains(sp.Name)
                select new
                {
                    ResourceName = ar.Name,
                    ResourceSecret = ar.Secret,
                }
            )
            .ToListAsync();

            var apiResources = apiResourceData.Select(i => new ApiResource
            {
                Name = i.ResourceName,
                ApiSecrets = new List<Secret>
                {
                    new Secret(i.ResourceSecret.ToString().Sha256())
                }
            }).ToList();

            return apiResources;
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<ApiResource>();
        }
    }

    public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
    {
        try
        {
            var apiScopeData = await dbContext.ApiScope.Where(i => scopeNames.Contains(i.Name)).ToListAsync();
            var apiScopes = apiScopeData.Select(i => new ApiScope
            {
                Name = i.Name,
                DisplayName = i.DisplayName
            }).ToList();

            return apiScopes;
        }
        catch(Exception ex )
        {
            Console.WriteLine(ex.Message);
            return new List<ApiScope>();
        }
    }

    public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        return new List<IdentityResource>();
    }

    public async Task<IdentityServer4.Models.Resources> GetAllResourcesAsync()
    {
        return new IdentityServer4.Models.Resources();
    }
}