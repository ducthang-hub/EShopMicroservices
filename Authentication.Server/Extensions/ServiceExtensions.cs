using System.Text;
using Authentication.Server.Domains;
using Authentication.Server.Persistence.DatabaseContext;
using Authentication.Server.ResourcesValidation;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        const string authSecret = "auth-signing-key";
        var key = Encoding.ASCII.GetBytes(authSecret);
        
        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
            {
                option.Authority = "https://localhost:5056";
                option.RequireHttpsMetadata = false;
                option.SaveToken = true;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
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