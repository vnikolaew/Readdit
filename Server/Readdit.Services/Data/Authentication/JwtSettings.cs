using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Readdit.Services.Data.Authentication;

public class JwtSettings
{
    public string AppSecret { get; set; }
    
    public string ValidIssuer { get; set; }
    
    public string ValidAudience { get; set; }
    
    public bool ValidateLifetime { get; set; }
    
    public bool ValidateActor { get; set; }
    
    public int RelativeExpirationInHours { get; set; }

    public TokenValidationParameters TokenValidationParameters
        => new()
        {
            ValidIssuer = ValidIssuer,
            ValidAudience = ValidAudience,
            IssuerSigningKey = SecurityKey,
            ValidateLifetime = ValidateLifetime,
            ValidateActor = ValidateActor,
        };

    public SymmetricSecurityKey SecurityKey
        => new(Encoding.UTF8.GetBytes(AppSecret));	 
}