using Readdit.Infrastructure.Models;

namespace Readdit.Services.Data.Authentication;

public interface IJwtService
{
    string GenerateTokenForUser(ApplicationUser user);
}