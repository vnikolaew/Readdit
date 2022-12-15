using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Readdit.Infrastructure.Common.Models;
using static Readdit.Common.GlobalConstants.User;

namespace Readdit.Infrastructure.Models;

public class ApplicationUser : IdentityUser, IAuditableEntity, IDeletableEntity
{
    [Required]
    [MinLength(FirstNameMinLength)]
    [MaxLength(FirstNameMaxLength)]
    public string FirstName { get; set; }
    
    [Required]
    [MinLength(LastNameMinLength)]
    [MaxLength(LastNameMaxLength)]
    public string LastName { get; set; }

    public UserProfile Profile { get; set; }
    
    public UserScore Score { get; set; }

    public DateTime CreatedOn { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public DateTime? DeletedOn { get; set; }

    public ICollection<UserCommunity> Communities { get; set; } = new List<UserCommunity>();
    
    public ICollection<Community> CommunitiesAdministrated { get; set; } = new List<Community>();

    public ICollection<CommunityPost> CommunityPosts { get; set; } = new List<CommunityPost>();

    public virtual ICollection<IdentityUserRole<string>> Roles { get; set; } = new List<IdentityUserRole<string>>();

    public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; } = new List<IdentityUserClaim<string>>();
}