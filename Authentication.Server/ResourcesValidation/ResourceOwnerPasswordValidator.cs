using Authentication.Server.Domains;
using Authentication.Server.Persistence.DatabaseContext;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Server.ResourcesValidation;

public class ResourceOwnerPasswordValidator(UserManager<User> userManager) : IResourceOwnerPasswordValidator
{
    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        try
        {
            var user = await userManager.Users.Where(i => i.UserName == context.UserName).FirstOrDefaultAsync();
            if( user == null )
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient);
                return;
            }

            var isPasswordMatched = await userManager.CheckPasswordAsync(user, context.Password);
            if (!isPasswordMatched)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient);
                return;
            }

            context.Result = new GrantValidationResult(subject: user.Id, GrantType.ResourceOwnerPassword);
        }
        catch (Exception ex)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            Console.WriteLine(ex.Message);
        }
    }
}