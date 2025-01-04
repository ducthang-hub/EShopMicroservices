namespace Authentication.Server.Domains;

public class ApiScope
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public List<ApiScopeResource> ApiScopeResources { get; set; }
    public List<ClientScope> ClientScopes { set; get; }

}