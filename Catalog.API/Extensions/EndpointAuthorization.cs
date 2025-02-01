using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Catalog.API.Extensions;

public static class EndpointAuthorization
{
    public static IServiceCollection AddEndpointAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var authority = configuration["Services:AuthenticationService:BaseAddress"];
        
        Console.WriteLine($"Authority {authority}");
        
        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
            {
                option.Authority = authority; // Authority will overrule the ValidIssuer
                option.RequireHttpsMetadata = false;
                option.SaveToken = true;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
            });


        services.AddAuthorizationBuilder()
            .AddPolicy("customer", policy =>
            {
                policy.RequireAuthenticatedUser();  
                policy.RequireClaim("client_id", "eshop-web"); 
                policy.RequireClaim("scope", "customer-scope");
                // policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", nameof(Common.SYSTEMROLES.user));
            });

        return services;
    }
    
    
}