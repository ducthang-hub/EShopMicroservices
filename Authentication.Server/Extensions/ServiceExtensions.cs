using Authentication.Server.Domains;
using Authentication.Server.Persistence.DatabaseContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Server.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<AuthDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("DatabaseConnection"));
            opt.UseOpenIddict();
        });
        
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();
        
        return services;
    }

    public static IServiceCollection ConfigOpenIddict(this IServiceCollection services)
    {
        services.AddOpenIddict()

            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                // Note: call ReplaceDefaultEntities() to replace the default entities.
                options.UseEntityFrameworkCore()
                    .UseDbContext<AuthDbContext>();
            })
            // Register the OpenIddict server components.
            .AddServer(options =>
            {
                // Enable the token endpoint.
                options.SetTokenEndpointUris("connect/token");

                // Enable the client credentials flow.
                // options.AllowClientCredentialsFlow();
                options.AllowPasswordFlow();
                
                // Accept anonymous clients (i.e clients that don't send a client_id).
                options.AcceptAnonymousClients();
                
                // Register the signing and encryption credentials.
                options.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();

                // Register the ASP.NET Core host and configure the ASP.NET Core options.
                options.UseAspNetCore()
                    .EnableTokenEndpointPassthrough();
            })
            // Register the OpenIddict validation components.
            .AddValidation(options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });
        
        services.AddAuthentication();
        services.AddAuthorization();

        return services;
    }
}