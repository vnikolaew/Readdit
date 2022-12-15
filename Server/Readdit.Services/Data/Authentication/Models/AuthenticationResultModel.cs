using SignalRChat.Services.Security.Jwt;

namespace Readdit.Services.Data.Authentication.Models;

public class AuthenticationResultModel
{
    public bool Succeeded { get; init; }
    
    public string? UserId { get; init; }
    
    public JwtToken? Token { get; init; }

    public IEnumerable<string> Errors { get; init; }
        = new List<string>();

    public static AuthenticationResultModel Success(string userId, JwtToken token)
        => new()
        {
            Succeeded = true,
            UserId = userId,
            Token = token
        };

    public static AuthenticationResultModel Failure(IEnumerable<string> errors)
        => new() { Errors = errors };
    
    public static AuthenticationResultModel Failure(string error)
        => new() { Errors = new []{ error } };
}