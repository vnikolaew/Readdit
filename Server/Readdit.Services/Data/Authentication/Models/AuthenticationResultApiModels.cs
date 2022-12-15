using SignalRChat.Services.Security.Jwt;

namespace Readdit.Services.Data.Authentication.Models;

public class AuthenticationResultSuccessModel
{
    public string UserId { get; init; }
    
    public JwtToken Token { get; init; }
}

public class AuthenticationResultErrorModel
{
    public IEnumerable<string> Errors { get; set; } = new List<string>();
}