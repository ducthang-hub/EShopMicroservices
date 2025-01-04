using Authentication.Server.Enums;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Server.Domains;

public class User : IdentityUser
{
    public AuthenProvider Provider { get; set; }

}