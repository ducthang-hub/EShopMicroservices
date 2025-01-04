using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;

namespace Authentication.Server.Feature.Commands.AuthCommands.Login;

public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginResponse : ErrorResponse{}

public class LoginCommand(LoginRequest payload) : ICommand<LoginResponse>
{
    public LoginRequest Payload { get; set; } = payload;
}