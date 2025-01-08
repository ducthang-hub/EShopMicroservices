using System.Net;
using Authentication.Server.Persistence.DatabaseContext;
using BuildingBlocks.CQRS;
using BuildingBlocks.Helpers;
using IdentityModel.Client;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Server.Features.Login;

public class LoginHandler
    (
        AuthDbContext dbContext,
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<LoginHandler> logger
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
            var user = await dbContext.User
                .Where(i => i.UserName == payload.UserName && i.PasswordHash == payload.Password)
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                response.Status = HttpStatusCode.NotFound;
                return response;
            }

            var origin = configuration["Application:UrlHttps"];
            
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