using System.Net;
using Catalog.API.DTOs;

namespace Catalog.API.Models.Responses;

public class GetValidationKeyResponse
{
    public HttpStatusCode Status { get; set; } = HttpStatusCode.InternalServerError;
    public string Message { get; set; } = default!;
    public AuthTokenDto Tokens { get; set; }
}