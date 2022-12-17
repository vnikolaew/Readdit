using AutoMapper;
using Readdit.Infrastructure.Models;
using Readdit.Services.Data.PostFeed.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Posts.Models;

public class PostCommentDetailsModel : IHaveCustomMappings
{
    public PostCommentAuthorModel Author { get; set; }
    
    public string Id { get; set; }
    
    public string Content { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public DateTime? ModifiedOn { get; set; }
    
    public int VoteScore { get; set; }
    
    public UserVoteModel? UserVote { get; set; }
    
    public void CreateMappings(IProfileExpression configuration)
    {
        string? currentUserId = null;
        configuration
            .CreateMap<PostComment, PostCommentDetailsModel>()
            .ForMember(pm => pm.UserVote,
                cfg =>
                    cfg.MapFrom(c => c.Votes.Where(v => v.UserId == currentUserId)
                        .Select(v => new UserVoteModel { Id = v.Id, Type = v.Type })
                        .FirstOrDefault()));
    }
}