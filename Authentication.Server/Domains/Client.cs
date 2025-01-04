namespace Authentication.Server.Domains;

public class Client
{
    public Guid Id { get; set; }
    public string ClientId { get; set; }
    public List<ClientScope> ClientScopes { get; set; }
    public List<ClientSecret> ClientSecrets { get; set; }
    public List<ClientGrantType> ClientGrantTypes { get; set; }   
}