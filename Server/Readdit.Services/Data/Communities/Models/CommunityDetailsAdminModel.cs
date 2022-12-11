using AutoMapper;
using Readdit.Infrastructure.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Communities.Models;

public class CommunityDetailsAdminModel : IHaveCustomMappings
{
    public string Id { get; set; }
    
    public string UserName { get; set; }

    public string ProfilePictureUrl { get; set; }
    
    public string ProfilePicturePublicId { get; set; }
    
    public void CreateMappings(IProfileExpression configuration)
        => configuration
            .CreateMap<ApplicationUser, CommunityDetailsAdminModel>()
            .ForMember(a => a.ProfilePictureUrl,
                cfg => cfg.MapFrom(u => u.Profile.ProfilePictureUrl))
            .ForMember(a => a.ProfilePicturePublicId,
                cfg => cfg.MapFrom(u => u.Profile.ProfilePicturePublicId));
}