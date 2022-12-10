using Readdit.Infrastructure.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.PostFeed.Models;

public class FeedCommunityModel : IMapFrom<Community>
{
    public string Id { get; set; }

    public string Name { get; set; }
    
    public string PictureUrl { get; set; }
    
    public string PicturePublicId { get; set; }
}