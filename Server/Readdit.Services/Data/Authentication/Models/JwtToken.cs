namespace SignalRChat.Services.Security.Jwt;

public class JwtToken
{
	public string Value { get; set; } = default!;
	
	public DateTime ExpiresAt { get; set; }
}