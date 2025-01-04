namespace Authentication.Server.Domains;

public class ApiScopeResource
{
    public Guid ApiResourceId { get; set; }
    public ApiResource ApiResource { get; set; }
    public Guid ApiScopeId { get; set; }
    public ApiScope ApiScope { get; set; }

}