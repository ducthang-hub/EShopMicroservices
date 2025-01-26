namespace Catalog.API.Models.Requests;

public class GetValidationKeyRequest
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ApiScope { get; set; }
    public string Scheme { get; set; }
}