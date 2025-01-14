using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;

namespace Authentication.Server.Features.Login;

public class LoginResponse : ErrorResponse
{
}

public class LoginRequest
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ApiScope { get; set; }
    public string Scheme { get; set; }
}

public class LoginCommand(LoginRequest payload) : ICommand<LoginResponse>
{
    public LoginRequest Payload { get; set; } = payload;
}