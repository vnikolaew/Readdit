using System.ComponentModel.DataAnnotations;
using Readdit.Infrastructure.Common.Models;
using Readdit.Infrastructure.Models.Enums;

namespace Readdit.Infrastructure.Models;

public class UserCommunity : IAuditableEntity, IDeletableEntity
{
    [Required]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }

    [Required]
    public string CommunityId { get; set; }

    public Community Community { get; set; }

    [Required]
    public UserCommunityStatus Status { get; set; }

    public DateTime CreatedOn { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public DateTime? DeletedOn { get; set; }
}