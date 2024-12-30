using BuildingBlocks.Contracts;
using MediatR;

namespace Authentication.Client.Features.Commands.Login;

public class LoginCommand(LoginRequest payload) : IRequest<LoginResponse>
{
    public LoginRequest Payload { get; set; } = payload;
}

public class LoginResponse : ErrorResponse
{
    
}

public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
