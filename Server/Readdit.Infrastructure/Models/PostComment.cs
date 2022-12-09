using System.ComponentModel.DataAnnotations;
using Readdit.Infrastructure.Common.Models;

namespace Readdit.Infrastructure.Models;

public class PostComment : BaseDeletableEntity<string>
{
    public PostComment()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Required]
    public string PostId { get; set; }

    public CommunityPost Post { get; set; }

    [Required]
    public string AuthorId { get; set; }

    public ApplicationUser Author { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Content { get; set; }
    
    [Required]
    public int VoteScore { get; set; }

    public ICollection<CommentVote> Votes { get; set; } = new List<CommentVote>();
}