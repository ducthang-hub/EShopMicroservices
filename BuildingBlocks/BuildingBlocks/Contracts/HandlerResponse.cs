using System.Net;

namespace BuildingBlocks.Contracts;

public class HandlerResponse
{
    public HttpStatusCode Status { get; set; } = HttpStatusCode.InternalServerError;
    public string Message { get; set; } = default!;
    public dynamic Data { get; set; }
}