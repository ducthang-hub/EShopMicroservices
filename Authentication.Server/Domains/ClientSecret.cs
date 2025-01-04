namespace Authentication.Server.Domains;

public class ClientSecret
{
    public Guid Id { get; set; }
    public string Secret { get; set; }
    public Guid ClientId { get; set; }
    
    public Client Client { get; set; }
}