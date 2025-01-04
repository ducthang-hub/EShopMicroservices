namespace Authentication.Server.Domains;

public class ClientGrantType
{
    public Guid Id { get; set; }
    public string GrantType { get; set; }
    public Guid ClientId { get; set; }
    
    public Client Client { get; set; }

}