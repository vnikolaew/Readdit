using System.ComponentModel.DataAnnotations;
using Readdit.Infrastructure.Common.Models;

namespace Readdit.Infrastructure.Models;

public class CommunityPost : BaseDeletableEntity<string>
{
    public CommunityPost()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Required]
    public string AuthorId { get; set; }

    public ApplicationUser Author { get; set; }

    [Required]
    public string CommunityId { get; set; }

    public Community Community { get; set; }

    public string? MediaUrl { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; }

    public ICollection<PostVote> Votes { get; set; } = new List<PostVote>();

    public ICollection<PostComment> Comments { get; set; } = new List<PostComment>();

    public ICollection<PostTag> Tags { get; set; } = new List<PostTag>();
}