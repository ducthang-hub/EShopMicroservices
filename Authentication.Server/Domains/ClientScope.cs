namespace Authentication.Server.Domains;

public class ClientScope
{
    public Guid ClientId { get; set; }
    public Guid ApiScopeId { get; set; }
    
    public ApiScope ApiScope { get; set; }
    public Client Client { get; set; }
}