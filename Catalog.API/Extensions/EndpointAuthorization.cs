using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Catalog.API.Extensions;

public static class EndpointAuthorization
{
    public static IServiceCollection AddEndpointAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var authority = configuration["IdentitySettings:Authority"];
        var authSecret = configuration["IdentitySettings:SigningKey"];

        if (authSecret != null)
        {
            var key = Encoding.ASCII.GetBytes(authSecret);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
                {
                    option.RequireHttpsMetadata = false;
                    option.SaveToken = true;
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidIssuer = authority,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
        }

        services.AddAuthorizationBuilder()
            .AddPolicy("student", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("client_id", "ewb-student-web"); 
                policy.RequireClaim("scope", "student-scope");
                // policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", nameof(Common.SYSTEMROLES.user));
            });

        return services;
    }
}