using Readdit.Services.Data.PostFeed.Enums;

namespace Readdit.Services.Data.Search;

public interface ISearchService
{
    Task<IEnumerable<T>> SearchCommunities<T>(string query);
    
    Task<IEnumerable<T>> SearchPosts<T>(string query, TimeRange range);
    
    Task<IEnumerable<T>> SearchUsers<T>(string query);
}