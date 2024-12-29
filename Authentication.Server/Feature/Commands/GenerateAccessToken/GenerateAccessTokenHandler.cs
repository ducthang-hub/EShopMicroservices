using System.Net;
using System.Security.Claims;
using Authentication.Server.Domains;
using BuildingBlocks.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Authentication.Server.Feature.Commands.GenerateAccessToken;

public class GenerateAccessTokenHandler
(
    ILogger<GenerateAccessTokenHandler> logger,
    SignInManager<User> signInManager,
    UserManager<User> userManager
)
: IRequestHandler<GenerateAccessTokenCommand, GenerateAccessTokenResponse>
{
    public async Task<GenerateAccessTokenResponse> Handle(GenerateAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var response = new GenerateAccessTokenResponse();
        
        try
        {
            var payload = request.Payload;
            if (payload == null || !payload.IsPasswordGrantType())
            {
                return response;
            }
            var user = await userManager.FindByNameAsync(payload.Username);
            if (user == null)
            {
                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The username/password couple is invalid."
                }!);

                response.Data = properties;
                response.Status = HttpStatusCode.Forbidden;
                return response;
            }
            
            var result = await signInManager.CheckPasswordSignInAsync(user, payload.Password, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The username/password couple is invalid."
                }!);

                response.Data = properties;
                response.Status = HttpStatusCode.Forbidden;
                return response;            
            }
            
            // Create the claims-based identity that will be used by OpenIddict to generate tokens.
            var identity = new ClaimsIdentity(
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);

            // Add the claims that will be persisted in the tokens.
            identity.SetClaim(Claims.Subject, await userManager.GetUserIdAsync(user))
                .SetClaim(Claims.Email, await userManager.GetEmailAsync(user))
                .SetClaim(Claims.Name, await userManager.GetUserNameAsync(user))
                .SetClaim(Claims.PreferredUsername, await userManager.GetUserNameAsync(user))
                .SetClaims(Claims.Role, [.. (await userManager.GetRolesAsync(user))]);

            // Set the list of scopes granted to the client application.
            identity.SetScopes(new[]
            {
                Scopes.OpenId,
                Scopes.Email,
                Scopes.Profile,
                Scopes.Roles
            }.Intersect(payload.GetScopes()));

            identity.SetDestinations(GetDestinations);
            response.Status = HttpStatusCode.OK;
            response.Data = new
            {
                ClaimsPrincipal = new ClaimsPrincipal(identity),
                Scheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
            };
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }

        return response;
    }
    
    private static IEnumerable<string> GetDestinations(Claim claim)
    {
        // Note: by default, claims are NOT automatically included in the access and identity tokens.
        // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
        // whether they should be included in access tokens, in identity tokens or in both.

        switch (claim.Type)
        {
            case Claims.Name or Claims.PreferredUsername:
                yield return Destinations.AccessToken;

                if (claim.Subject.HasScope(Permissions.Scopes.Profile))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Email:
                yield return Destinations.AccessToken;

                if (claim.Subject.HasScope(Permissions.Scopes.Email))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Role:
                yield return Destinations.AccessToken;

                if (claim.Subject.HasScope(Permissions.Scopes.Roles))
                    yield return Destinations.IdentityToken;

                yield break;

            // Never include the security stamp in the access and identity tokens, as it's a secret value.
            case "AspNet.Identity.SecurityStamp": yield break;

            default:
                yield return Destinations.AccessToken;
                yield break;
        }
    }
}