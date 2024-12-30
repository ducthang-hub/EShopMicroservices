using System.Net;
using BuildingBlocks.Helpers;
using MediatR;
using OpenIddict.Client;

namespace Authentication.Client.Features.Commands.Login;

public class LoginHandler
    (
        IServiceProvider serviceProvider,
        ILogger<LoginHandler> logger
    )
    : IRequestHandler<LoginCommand, LoginResponse>
{
    
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var response = new LoginResponse();
        var payload = request.Payload;
        try
        {
            // using var client = serviceProvider.GetService<HttpClient>();
            // var registerResponse = await client.PostAsJsonAsync("https://localhost:5056/Account/Register", new { payload.UserName, payload.Password });
            //
            // // Ignore 409 responses, as they indicate that the account already exists.
            // if (registerResponse.StatusCode == HttpStatusCode.Conflict)
            // {
            //     return response;
            // }
            //
            // response.Status = HttpStatusCode.OK;
            // response.Data = registerResponse;
            var service = serviceProvider.GetRequiredService<OpenIddictClientService>();

            var result = await service.AuthenticateWithPasswordAsync(new()
            {
                Username = payload.UserName,
                Password = payload.Password
            });
            response.Data = result;
            response.Status = HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }

        return response;
    }
}