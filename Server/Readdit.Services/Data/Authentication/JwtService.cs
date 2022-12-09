using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Readdit.Common;
using Readdit.Infrastructure.Models;

namespace Readdit.Services.Data.Authentication;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
    }

    public string GenerateTokenForUser(ApplicationUser user)
    {
        var credentials = new SigningCredentials(_jwtSettings.SecurityKey,
            SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.UserName),
                new(GlobalConstants.Claims.CountryClaim, user.Country.Name)
            }),
            Expires = DateTime.Now.AddHours(_jwtSettings.RelativeExpirationInHours),
            SigningCredentials = credentials,
            Issuer = _jwtSettings.ValidIssuer,
            Audience = _jwtSettings.ValidAudience,
            IssuedAt = DateTime.Now
        };
		
        return GetTokenFromDescriptor(tokenDescriptor);
    }
    
    private static string GetTokenFromDescriptor(SecurityTokenDescriptor descriptor)
    {
        var securityTokenHandler = new JwtSecurityTokenHandler();
        return securityTokenHandler.WriteToken(securityTokenHandler.CreateToken(descriptor));
    }
}