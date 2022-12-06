using System.ComponentModel.DataAnnotations;
using Readdit.Infrastructure.Common.Models;
using Readdit.Infrastructure.Models.Enums;

namespace Readdit.Infrastructure.Models;

public class PostVote : BaseDeletableEntity<string>
{
    public PostVote()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Required]
    public string PostId { get; set; }

    public CommunityPost Post { get; set; }

    [Required]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }

    [Required]
    public VoteType Type { get; set; }
}