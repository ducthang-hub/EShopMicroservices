using BuildingBlocks.HttpClient.Extension;
using BuildingBlocks.HttpClient.Implement;
using Carter;
using Catalog.API.DTOs;
using Catalog.API.Models.Requests;
using Catalog.API.Models.Responses;

namespace Catalog.API.Features.Queries.ProductQueries.GetValidationKey;

public class GetValidationKeyEndpoint : ICarterModule
{
    // todo: remove this endpoint when done testing
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("validation-key", async (
            GetValidationKeyRequest request,
            ILogger<GetValidationKeyEndpoint> logger,
            ICustomHttpClient<AuthenticationService> httpClient,
            CancellationToken cancellationToken
        ) =>
        {
            try
            {
                var response = await httpClient.PostAsync<GetValidationKeyRequest, GetValidationKeyResponse>("authen/login", request, cancellationToken);
                var result = response?.Tokens;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new AuthTokenDto();
            }
        });
    }
}