using AutoMapper;
using Readdit.Infrastructure.Models;
using Readdit.Services.Data.PostFeed.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Posts.Models;

public class PostDetailsModel : IHaveCustomMappings
{
    public string Id { get; set; }
    
    public FeedPostAuthorModel Author { get; set; }

    public FeedCommunityModel Community { get; set; }

    public string? MediaUrl { get; set; }
    
    public string? MediaPublicId { get; set; }

    public string Title { get; set; }
    
    public string Content { get; set; }

    public int VoteScore { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
    
    public UserVoteModel? UserVote { get; set; }

    public ICollection<PostCommentDetailsModel> Comments { get; set; }
        = new List<PostCommentDetailsModel>();

    public ICollection<string> Tags { get; set; } = new List<string>();

    public void CreateMappings(IProfileExpression configuration)
    {
        string? currentUserId = null;
        configuration
            .CreateMap<CommunityPost, PostDetailsModel>()
            .ForMember(pm => pm.Tags,
                cfg => cfg.MapFrom(p => p.Tags.Select(t => t.Tag.Name)))
            .ForMember(pm => pm.UserVote,
            cfg =>
                cfg.MapFrom(p => p.Votes.Where(v => v.UserId == currentUserId)
                    .Select(v => new UserVoteModel { Id = v.Id, Type = v.Type })
                    .FirstOrDefault()));

    }
}