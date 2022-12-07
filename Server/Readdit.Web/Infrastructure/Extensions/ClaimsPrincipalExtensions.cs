using System.Security.Claims;

namespace Readdit.Web.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetId(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.NameIdentifier);
    
    public static string? GetUserName(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.Name);
    
    public static string? GetEmail(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.Email);
}