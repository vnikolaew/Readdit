using Readdit.Infrastructure.Models;
using SignalRChat.Services.Security.Jwt;

namespace Readdit.Services.Data.Authentication;

public interface IJwtService
{
    JwtToken GenerateTokenForUser(ApplicationUser user, IEnumerable<string>? roles);
}