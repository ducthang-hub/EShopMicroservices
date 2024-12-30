using System.Threading.Tasks;
using Authentication.Server.Domains;
using Authentication.Server.Feature.Commands.GenerateAccessToken;
using BuildingBlocks.Helpers;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Server;

namespace Authentication.Server.Controllers;

public class AuthorizationController
(
    ILogger<AuthorizationController> logger,
    IMediator mediator 
)
    : ControllerBase
{
    [HttpPost("~/connect/token"), IgnoreAntiforgeryToken, Produces("application/json")]
    public async Task<GenerateAccessTokenResponse> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();
        var response = await mediator.Send(new GenerateAccessTokenCommand(request));
        return response;
    }
}