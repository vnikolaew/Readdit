using AutoMapper;
using Readdit.Infrastructure.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Search.Models;

public class UserSearchModel : IHaveCustomMappings
{
    public string Id { get; set; }
    
    public string UserName { get; set; }
    
    public string ProfilePictureUrl { get; set; }
    
    public string ProfilePicturePublicId { get; set; }

    public int PostScore { get; set; }
    
    public int CommentScore { get; set; }
    
    public string AboutContent { get; set; }

    public void CreateMappings(IProfileExpression configuration)
        => configuration
            .CreateMap<ApplicationUser, UserSearchModel>()
            .ForMember(sm => sm.ProfilePictureUrl,
                cfg => cfg.MapFrom(u => u.Profile.ProfilePictureUrl))
            .ForMember(sm => sm.ProfilePicturePublicId,
                cfg => cfg.MapFrom(u => u.Profile.ProfilePicturePublicId))
            .ForMember(sm => sm.AboutContent,
                cfg => cfg.MapFrom(u => u.Profile.AboutContent))
            .ForMember(sm => sm.PostScore,
                cfg => cfg.MapFrom(u => u.Score.PostsScore))
            .ForMember(sm => sm.CommentScore,
                cfg => cfg.MapFrom(u => u.Score.CommentsScore));
}