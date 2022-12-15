using AutoMapper;
using Readdit.Infrastructure.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.PostFeed.Models;

public class FeedCommunityPostModel : IHaveCustomMappings
{
    public FeedPostAuthorModel Author { get; set; }

    public FeedCommunityModel Community { get; set; }

    public string Id { get; set; }
    
    public string? MediaUrl { get; set; }
    
    public string? MediaPublicId { get; set; }

    public string Title { get; set; }
    
    public string Content { get; set; }

    public DateTime CreatedOn { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
    
    public int VoteScore { get; set; }
    
    public int CommentsCount { get; set; }
    
    public UserVoteModel? UserVote { get; set; }
    
    public void CreateMappings(IProfileExpression configuration)
    {
        string? currentUserId = null;
        configuration
            .CreateMap<CommunityPost, FeedCommunityPostModel>()
            .ForMember(fp => fp.CommentsCount,
                cfg => cfg.MapFrom(p => p.Comments.Count))
            .ForMember(pm => pm.UserVote,
                cfg =>
                    cfg.MapFrom(p => p.Votes.Where(v => v.UserId == currentUserId)
                        .Select(v => new UserVoteModel { Id = v.Id, Type = v.Type })
                        .FirstOrDefault()));
    }
}