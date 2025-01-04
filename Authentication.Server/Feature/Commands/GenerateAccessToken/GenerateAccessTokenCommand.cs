using BuildingBlocks.Contracts;
using MediatR;

namespace Authentication.Server.Feature.Commands.GenerateAccessToken;

public class GenerateAccessTokenCommand() : IRequest<GenerateAccessTokenResponse>
{
}

public class GenerateAccessTokenResponse : ErrorResponse;