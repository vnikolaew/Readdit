using System.ComponentModel.DataAnnotations;
using Readdit.Infrastructure.Common.Models;
using Readdit.Infrastructure.Models.Enums;

namespace Readdit.Infrastructure.Models;

public class Community : BaseDeletableEntity<string>
{
    public Community()
    {
        Id = Guid.NewGuid().ToString();
    }

    public ICollection<UserCommunity> Members { get; set; } = new List<UserCommunity>();

    [Required]
    public string AdminId { get; set; }
    
    public ApplicationUser Admin { get; set; }

    [Required]
    public CommunityType Type { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    [Url]
    public string PictureUrl { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string PicturePublicId { get; set; }

    public ICollection<CommunityTag> Tags { get; set; } = new List<CommunityTag>();

    public ICollection<CommunityPost> Posts { get; set; } = new List<CommunityPost>();
}