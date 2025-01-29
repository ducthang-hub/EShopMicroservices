using BuildingBlocks.Contracts;
using MediatR;

namespace Authentication.Server.Features.RegisterUser;

public class RegisterUserResponse : ErrorResponse
{
}

public class RegisterUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
}

public class RegisterUserCommand(RegisterUserRequest payload) : IRequest<RegisterUserResponse>
{
    public RegisterUserRequest Payload { get; set; } = payload;
}