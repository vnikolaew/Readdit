using AutoMapper;
using Readdit.Infrastructure.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Search.Models;

public class CommunitySearchModel : IHaveCustomMappings
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public int MembersCount { get; set; }
    
    public string PictureUrl { get; set; }
    
    public string PicturePublicId { get; set; }
    
    public IEnumerable<string> Tags { get; set; } = new List<string>();
    
    public bool HasUserJoined { get; set; }
    
    public void CreateMappings(IProfileExpression configuration)
    {
        string? currentUserId = null;
        configuration
            .CreateMap<Community, CommunitySearchModel>()
            .ForMember(sm => sm.MembersCount,
                cfg => cfg.MapFrom(c => c.Members.Count))
            .ForMember(sm => sm.HasUserJoined,
                cfg =>
                    cfg.MapFrom(c => c.Members.Any(m => m.UserId == currentUserId)))
            .ForMember(sm => sm.Tags,
                cfg => cfg.MapFrom(c => c.Tags.Select(t => t.Tag.Name)));
    }
}