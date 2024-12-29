using BuildingBlocks.Contracts;
using MediatR;
using OpenIddict.Abstractions;

namespace Authentication.Server.Feature.Commands.GenerateAccessToken;

public class GenerateAccessTokenCommand(OpenIddictRequest? payload) : IRequest<GenerateAccessTokenResponse>
{
    public OpenIddictRequest? Payload { get; set; } = payload;
}

public class GenerateAccessTokenResponse : ErrorResponse;