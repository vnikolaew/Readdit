using System.ComponentModel.DataAnnotations;

namespace Readdit.Infrastructure.Models;

public class UserCommunity
{
    [Required]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }

    [Required]
    public string CommunityId { get; set; }

    public Community Community { get; set; }
}