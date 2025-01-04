using Authentication.Server.Persistence.DatabaseContext;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Server.ResourcesValidation;

public class ResourceOwnerPasswordValidator(AuthDbContext dbContext) : IResourceOwnerPasswordValidator
{
    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        try
        {
            //todo: create hash password service
            var user = await dbContext.User
                .Where(i => i.UserName == context.UserName && i.PasswordHash == context.Password)
                .FirstOrDefaultAsync();

            if( user == null )
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
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