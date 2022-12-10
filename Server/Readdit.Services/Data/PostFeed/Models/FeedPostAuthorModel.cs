using Readdit.Infrastructure.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.PostFeed.Models;

public class FeedPostAuthorModel : IMapFrom<ApplicationUser>
{
    public string Id { get; set; }

    public string UserName { get; set; }
}