using AutoMapper;
using Readdit.Infrastructure.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Users.Models;

public class UserDetailsModel : IHaveCustomMappings
{
    public string Id { get; set; }
    
    public string UserName { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Email { get; set; }
    
    public DateTime CreatedOn { get; set; }
    
    public UserProfileModel Profile { get; set; }
    
    public int PostsScore { get; set; }
    
    public int CommentsScore { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration
            .CreateMap<ApplicationUser, UserDetailsModel>()
            .ForMember(um => um.PostsScore,
                cfg => cfg.MapFrom(u => u.Score.PostsScore))
            .ForMember(um => um.CommentsScore,
                cfg => cfg.MapFrom(u => u.Score.CommentsScore));
    }
}