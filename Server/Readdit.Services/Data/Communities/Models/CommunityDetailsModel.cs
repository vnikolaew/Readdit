using AutoMapper;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Communities.Models;

public class CommunityDetailsModel : IHaveCustomMappings
{
    public CommunityDetailsAdminModel Admin { get; set; }

    public CommunityType Type { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string PictureUrl { get; set; }
    
    public string PicturePublicId { get; set; }
    
    public bool HasUserJoined { get; set; }

    public ICollection<string> Tags { get; set; } = new List<string>();

    public int MembersCount { get; set; }

    public DateTime CreatedOn { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        string? currentUserId = null;
        configuration
            .CreateMap<Community, CommunityDetailsModel>()
            .ForMember(cdm => cdm.Tags,
                cfg => cfg.MapFrom(c => c.Tags.Select(t => t.Tag.Name)))
            .ForMember(cdm => cdm.MembersCount,
                cfg => cfg.MapFrom(c => c.Members.Count))
            .ForMember(cdm => cdm.HasUserJoined,
                cfg =>
                    cfg.MapFrom((c) => c.Members.Any(uc => uc.UserId == currentUserId)));
    }
}