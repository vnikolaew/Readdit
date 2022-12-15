using AutoMapper;
using Readdit.Infrastructure.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.UserCommunities.Models;

public class UserCommunityModel : IHaveCustomMappings
{
    public string Id { get; set; }
    
    public string Name { get; set; }
        
    public string PictureUrl { get; set; } 
    
    public string PicturePublicId { get; set; }

    public int MembersCount { get; set; }
    
    public void CreateMappings(IProfileExpression configuration)
    {
        configuration
            .CreateMap<Community, UserCommunityModel>()
            .ForMember(ucm => ucm.MembersCount,
                cfg => cfg.MapFrom(c => c.Members.Count));
    }
}