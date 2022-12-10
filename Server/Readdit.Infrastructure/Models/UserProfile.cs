using System.ComponentModel.DataAnnotations;
using Readdit.Infrastructure.Common.Models;
using Readdit.Infrastructure.Models.Enums;
using static Readdit.Common.GlobalConstants.UserProfile;

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
    
    [Required]
    [MaxLength(100)]
    public string ProfilePicturePublicId { get; set; }

    [MaxLength(AboutContentMaxLength)]
    public string? AboutContent { get; set; }
    
    public Gender Gender { get; set; }

    public string CountryId { get; set; }
    
    [Required]
    public Country Country { get; set; }
}