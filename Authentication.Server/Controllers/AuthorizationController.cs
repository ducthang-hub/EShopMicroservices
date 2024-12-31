using Authentication.Server.Feature.Commands.GenerateAccessToken;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Server.Controllers;

public class AuthorizationController
    (
        ILogger<AuthorizationController> logger,
        IMediator mediator 
    )
    : Controller
{
    [HttpPost("~/connect/token"), IgnoreAntiforgeryToken, Produces("application/json")]
    public async Task<GenerateAccessTokenResponse> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        var response = await mediator.Send(new GenerateAccessTokenCommand(request));
        return response;
    }
}