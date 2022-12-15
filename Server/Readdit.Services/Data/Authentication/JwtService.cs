using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using CloudinaryDotNet.Actions;
using Microsoft.IdentityModel.Tokens;
using Readdit.Common;
using Readdit.Infrastructure.Models;
using SignalRChat.Services.Security.Jwt;

namespace Readdit.Services.Data.Authentication;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(JwtSettings jwtSettings)
        => _jwtSettings = jwtSettings;

    public JwtToken GenerateTokenForUser(
        ApplicationUser user, IEnumerable<string>? roles)
    {
        var credentials = new SigningCredentials(
            _jwtSettings.SecurityKey,
            SecurityAlgorithms.HmacSha256);

        var userClaims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName),
            new(GlobalConstants.Claims.CountryClaim, user.Profile.Country.Name)
        };
        if (roles is not null)
        {
            userClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(userClaims),
            Expires = DateTime.Now.AddHours(_jwtSettings.RelativeExpirationInHours),
            SigningCredentials = credentials,
            Issuer = _jwtSettings.ValidIssuer,
            Audience = _jwtSettings.ValidAudience,
            IssuedAt = DateTime.Now
        };
		
        return GetTokenFromDescriptor(tokenDescriptor);
    }
    
    private static JwtToken GetTokenFromDescriptor(SecurityTokenDescriptor descriptor)
    {
        var securityTokenHandler = new JwtSecurityTokenHandler();
        var token = securityTokenHandler.CreateToken(descriptor);

        return new JwtToken
        {
            Value = securityTokenHandler.WriteToken(token),
            ExpiresAt = token.ValidTo
        };
    }
}