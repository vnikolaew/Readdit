using AutoMapper;
using Readdit.Infrastructure.Models;
using Readdit.Services.Data.PostFeed.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Posts.Models;

public class CommunityPostModel : IHaveCustomMappings
{
    public string Id { get; set; }
    
    public string CommunityId { get; set; }
    
    public string AuthorId { get; set; }
    
    public string AuthorUserName { get; set; }
    
    public string? MediaUrl { get; set; }
    
    public string? MediaPublicId { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
    
    public string Title { get; set; }
    
    public string Content { get; set; }

    public int VoteScore { get; set; }
    
    public UserVoteModel? UserVote { get; set; }
    
    public int CommentsCount { get; set; }

    public IEnumerable<string> Tags { get; set; } = new List<string>();

    public void CreateMappings(IProfileExpression configuration)
    {
        string? currentUserId = null;
        configuration
            .CreateMap<CommunityPost, CommunityPostModel>()
            .ForMember(pm => pm.CommentsCount,
                cfg => cfg.MapFrom(p => p.Comments.Count))
            .ForMember(pm => pm.Tags,
                cfg => cfg.MapFrom(p => p.Tags.Select(t => t.Tag.Name)))
            .ForMember(pm => pm.UserVote,
                cfg =>
                    cfg.MapFrom(p => p.Votes.Where(v => v.UserId == currentUserId)
                        .Select(v => new UserVoteModel { Id = v.Id, Type = v.Type })
                        .FirstOrDefault()));
    }
}