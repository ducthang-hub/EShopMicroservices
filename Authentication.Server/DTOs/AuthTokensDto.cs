namespace Authentication.Server.DTOs;

public class AuthTokensDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}