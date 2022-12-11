using System.ComponentModel.DataAnnotations;
using Readdit.Infrastructure.Common.Models;
using static Readdit.Common.GlobalConstants.Post;

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

    [Url]
    public string? MediaUrl { get; set; }
    
    [Required]
    public string MediaPublicId { get; set; }

    [Required]
    [MinLength(TitleMinLength)]
    [MaxLength(TitleMaxLength)]
    public string Title { get; set; }
    
    [Required]
    [MinLength(ContentMinLength)]
    [MaxLength(ContentMaxLength)]
    public string Content { get; set; }

    [Required]
    public int VoteScore { get; set; }

    public ICollection<PostVote> Votes { get; set; } = new List<PostVote>();

    public ICollection<PostComment> Comments { get; set; } = new List<PostComment>();

    public ICollection<PostTag> Tags { get; set; } = new List<PostTag>();
}