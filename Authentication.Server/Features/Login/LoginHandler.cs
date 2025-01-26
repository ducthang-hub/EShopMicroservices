using System.Net;
using Authentication.Server.DTOs;
using BuildingBlocks.CQRS;
using BuildingBlocks.Helpers;
using IdentityModel.Client;

namespace Authentication.Server.Features.Login;

public class LoginHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginHandler> _logger;
    public LoginHandler
    (
        IConfiguration configuration,
        ILogger<LoginHandler> logger
    )
    {
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        _httpClient = new HttpClient(clientHandler);

        _configuration = configuration;
        _logger = logger;
    }
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var payload = request.Payload;
        const string funcName = $"{nameof(LoginHandler)} =>";

        var response = new LoginResponse();
        
        try
        {
            var requestScheme = payload.Scheme;
            var origin = _configuration[$"Application:{requestScheme}"];
            
            Console.WriteLine($"{funcName} client origin {origin}");
            
            var tokenResponse = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
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
            var tokens = new AuthTokensDto()
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
            };

            response.Status = HttpStatusCode.OK;
            response.Tokens = tokens;
            return response;
        }
        catch(Exception ex)
        {
            ex.LogError(_logger);
        }

        return response;
    }
}