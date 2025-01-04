using Authentication.Server.Domains;
using BuildingBlocks.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;


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
            
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }

        return response;
    }
}