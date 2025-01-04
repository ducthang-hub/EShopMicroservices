namespace Authentication.Server.Domains;

public class ApiResource
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid Secret { get; set; }
    public List<ApiScopeResource> ApiScopeResources { get; set; }

}