using System.Net;
using Authentication.Server.Domains;
using Authentication.Server.Persistence.DatabaseContext;
using BuildingBlocks.CQRS;
using BuildingBlocks.Helpers;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Server.Features.Login;

public class LoginHandler
    (
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<LoginHandler> logger,
        IHttpContextAccessor httpContextAccessor
    )
    : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var payload = request.Payload;
        const string funcName = $"{nameof(LoginHandler)} =>";

        var response = new LoginResponse();
        
        try
        {
            var requestScheme = payload.Scheme;
            var origin = configuration[$"Application:{requestScheme}"];
            
            Console.WriteLine($"{funcName} client origin {origin}");
            
            var tokenResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = $"{origin}/connect/token",
                ClientId = payload.ClientId,
                Scope = "offline_access " + payload.ApiScope,
                ClientSecret = payload.ClientSecret,
                UserName = payload.UserName,
                Password = payload.Password,
            }, cancellationToken: cancellationToken);
            
            if (!string.IsNullOrEmpty(tokenResponse.Error))
            {
                Console.WriteLine($"{funcName} Token Response Error: {tokenResponse.Error}");
                response.Message =
                    $"ErrorDescription: {tokenResponse.ErrorDescription}\nErrorType: {tokenResponse.ErrorType.ToString()}\nRawJson: {tokenResponse.Raw}";
                response.Status = HttpStatusCode.BadRequest;
                return response;
            }

            if (tokenResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                response.Status = HttpStatusCode.BadRequest;
                response.Data = tokenResponse;
                return response;
            }
            var output = new
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
            };

            response.Status = HttpStatusCode.OK;
            response.Data = output;
            return response;
        }
        catch(Exception ex)
        {
            ex.LogError(logger);
        }

        return response;
    }
}