using System.Net;
using Basket.API.Models.DTOs;

namespace Basket.API.Models.Responses;

public class GetProductResponse
{
    public HttpStatusCode Status { get; set; } = HttpStatusCode.InternalServerError;
    public string Message { get; set; } = default!;
    public IEnumerable<ProductDto> Products { get; set; }
}