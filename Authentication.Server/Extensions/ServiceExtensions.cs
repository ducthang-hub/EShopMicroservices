using Authentication.Server.Domains;
using Authentication.Server.Persistence.DatabaseContext;
using Authentication.Server.ResourcesValidation;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Server.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<AuthDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("DatabaseConnection"));
        });
        
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

        
        return services;
    }

    public static IServiceCollection ConfigAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer();

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection ConfigIdentityServer(this IServiceCollection services)
    {
        services.AddIdentityServer(opts =>
        {
            opts.Events.RaiseErrorEvents = true;
            opts.Events.RaiseInformationEvents = true;
            opts.Events.RaiseFailureEvents = true;
            opts.Events.RaiseSuccessEvents = true;
            opts.EmitStaticAudienceClaim = true;
        })
        .AddClientStore<ClientStore>()
        .AddResourceStore<ResourceStore>()
        .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
        .AddProfileService<ProfileService>()
        .AddDeveloperSigningCredential();

        services.AddTransient<IProfileService, ProfileService>();
        services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
        
        return services;
    }
    
}