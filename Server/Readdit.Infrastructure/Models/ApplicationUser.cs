using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Readdit.Infrastructure.Common.Models;
using Readdit.Infrastructure.Models.Enums;

namespace Readdit.Infrastructure.Models;

public class ApplicationUser : IdentityUser, IAuditableEntity, IDeletableEntity
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    public Gender Gender { get; set; }

    [Required]
    public Country Country { get; set; }

    public virtual UserProfile Profile { get; set; }

    public DateTime CreatedOn { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public DateTime? DeletedOn { get; set; }

    public ICollection<UserCommunity> Communities { get; set; } = new List<UserCommunity>();
    
    public ICollection<Community> CommunitiesAdministrated { get; set; } = new List<Community>();

    public ICollection<CommunityPost> CommunityPosts { get; set; } = new List<CommunityPost>();
}