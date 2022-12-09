using Readdit.Services.Data.PostFeed.Enums;

namespace Readdit.Services.Data.PostFeed;

public interface IPostFeedService
{
    Task<IEnumerable<T>> GetMostRecentForUserAsync<T>(string userId);

    Task<IEnumerable<T>> GetBestVotedForUserAsync<T>(
        string userId,
        TimeRange range);
}