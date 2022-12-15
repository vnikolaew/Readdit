using AutoMapper;
using Readdit.Infrastructure.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Search.Models;

public class PostSearchModel : IHaveCustomMappings
{
    public string Id { get; set; }
    
    public string Title { get; set; }
    
    public string Content { get; set; }
    
    public string AuthorUserName { get; set; }
    
    public string AuthorId { get; set; }
    
    public string CommunityId { get; set; }
    
    public string CommunityName { get; set; }
    
    public string CommunityPictureUrl { get; set; }
    
    public string CommunityPicturePublicId { get; set; }
    
    public int VoteScore { get; set; }
    
    public int CommentsCount { get; set; }
    
    public string? MediaUrl { get; set; }
    
    public string? MediaPublicId { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public DateTime? ModifiedOn { get; set; }

    public void CreateMappings(IProfileExpression configuration)
        => configuration
            .CreateMap<CommunityPost, PostSearchModel>()
            .ForMember(sm => sm.CommentsCount,
                cfg =>
                    cfg.MapFrom(p => p.Comments.Count));
}