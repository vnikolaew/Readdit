using AutoMapper;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Users.Models;

public class UserProfileModel : IHaveCustomMappings
{
    public string PictureUrl { get; set; }
    
    public string PicturePublicId { get; set; }

    public string? AboutContent { get; set; }
    
    public Gender Gender { get; set; }

    public string CountryName { get; set; }
    
    public string CountryCode { get; set; }

    public void CreateMappings(IProfileExpression configuration)
        => configuration
            .CreateMap<UserProfile, UserProfileModel>()
            .ForMember(pm => pm.PictureUrl,
                cfg => cfg.MapFrom(p => p.ProfilePictureUrl))
            .ForMember(pm => pm.PicturePublicId,
                cfg => cfg.MapFrom(p => p.ProfilePicturePublicId))
            .ForMember(pm => pm.CountryName,
                cfg => cfg.MapFrom(p => p.Country.Name))
            .ForMember(pm => pm.CountryCode,
                cfg => cfg.MapFrom(p => p.Country.Code));
}