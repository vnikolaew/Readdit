using System.ComponentModel.DataAnnotations;
using Readdit.Infrastructure.Common.Models;

namespace Readdit.Infrastructure.Models;

public class UserProfile : BaseDeletableEntity<string>
{
    public UserProfile()
    {
        Id = Guid.NewGuid().ToString();
    }
    
    [Required]
    public string UserId { get; set; }
    
    public ApplicationUser User { get; set; }

    [Required]
    [Url]
    public string ProfilePictureUrl { get; set; }

    [MaxLength(200)]
    public string? AboutContent { get; set; }
}