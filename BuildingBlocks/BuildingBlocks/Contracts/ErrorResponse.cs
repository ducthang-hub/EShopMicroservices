using System.Net;

namespace BuildingBlocks.Contracts;

public class ErrorResponse
{
    public HttpStatusCode Status { get; set; } = HttpStatusCode.InternalServerError;
    public string Message { get; set; } = default!;
    public dynamic Data { get; set; }
}