using System.Security.Claims;
using Authentication.Server.Constants;
using Authentication.Server.Domains;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Server.ResourcesValidation;

public class ProfileService
    (
        IUserClaimsPrincipalFactory<User> claimsFactory,
        UserManager<User> userManager    
    )
    : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        try
        {
            var sub = context.Subject.GetSubjectId();
            var user = await userManager.FindByIdAsync(sub);

            var principal = await claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            claims.Add(new Claim(CustomClaimTypes.UserId, user.Id ?? string.Empty));
            claims.Add(new Claim(CustomClaimTypes.Email, user.Email ?? string.Empty));
            claims.Add(new Claim(CustomClaimTypes.Phone, user.PhoneNumber ?? string.Empty));
            claims.Add(new Claim(CustomClaimTypes.Provider, user.Provider.ToString()));
            claims.Add(new Claim(CustomClaimTypes.SecurityStamp, user.SecurityStamp ?? string.Empty));

            context.IssuedClaims = claims;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var subId = context.Subject.GetSubjectId();
        var user = await userManager.FindByIdAsync(subId);
        context.IsActive = user != null;
    }
}