using System.Text;
using BuildingBlocks.Helpers;
using Carter;
using Catalog.API.DTOs;
using Catalog.API.Models.Requests;
using Catalog.API.Models.Responses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.API.Features.Queries.ProductQueries.GetValidationKey;

public class GetValidationKeyEndpoint : ICarterModule
{
    // todo: remove this endpoint when done testing
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("validation-key", async (
            // HttpClient httpClient,
            GetValidationKeyRequest request,
            IConfiguration configuration,
            ILogger<GetValidationKeyEndpoint> logger
        ) =>
        {
            try
            {
                var authServer = configuration["Services:Authentication.Server"];
                var clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

                var requestAsString = JsonHelper.Serialize(request);
                var content = new StringContent(requestAsString, Encoding.UTF8, "application/json");
                using var httpClient = new HttpClient(clientHandler);
                httpClient.BaseAddress = new Uri(authServer!);

                var getValidationKeyResponse = await httpClient.PostAsync("authen/login", content);
                getValidationKeyResponse.EnsureSuccessStatusCode();

                var responseAsString = await getValidationKeyResponse.Content.ReadAsStringAsync();
                var result = JsonHelper.Deserialize<GetValidationKeyResponse>(responseAsString);
                return result.Tokens;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new AuthTokenDto()
                {
                    AccessToken = "surprise",
                    RefreshToken = "mother fucker"
                };
            }
        });
    }
}