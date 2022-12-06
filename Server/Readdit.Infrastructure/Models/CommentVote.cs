using System.ComponentModel.DataAnnotations;
using Readdit.Infrastructure.Common.Models;
using Readdit.Infrastructure.Models.Enums;

namespace Readdit.Infrastructure.Models;

public class CommentVote : BaseDeletableEntity<string>
{
    public CommentVote()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Required]
    public string CommentId { get; set; }

    public PostComment Comment { get; set; }

    [Required]
    public string UserId { get; set; }
    
    public ApplicationUser User { get; set; }
    
    [Required]
    public VoteType Type { get; set; }
}