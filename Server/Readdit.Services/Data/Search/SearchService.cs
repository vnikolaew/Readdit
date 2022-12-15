using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Services.Data.PostFeed.Enums;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Search;

public class SearchService : ISearchService
{
    private readonly IRepository<Community> _communities;
    private readonly IRepository<CommunityPost> _posts;
    private readonly IRepository<ApplicationUser> _users;

    public SearchService(
        IRepository<Community> communities,
        IRepository<CommunityPost> posts,
        IRepository<ApplicationUser> users)
    {
        _communities = communities;
        _posts = posts;
        _users = users;
    }

    public async Task<IEnumerable<T>> SearchCommunities<T>(string query)
    {
        var communities = await _communities
            .AllAsNoTracking()
            .Where(c => EF.Functions.Like(c.Name, $"%{query}%") ||
                        EF.Functions.Like(c.Description, $"%{query}%"))
            .OrderByDescending(c => c.Members.Count)
            .To<T>()
            .ToListAsync();
        
        return communities;
    }

    public async Task<IEnumerable<T>> SearchPosts<T>(string query, TimeRange range)
    {
        var startDate = range.GetStartDate();
        var posts = await _posts
            .AllAsNoTracking()
            .Where(p => EF.Functions.Like(p.Title, $"%{query}%")
                        && p.CreatedOn > startDate)
            .OrderByDescending(p => p.CreatedOn)
            .To<T>()
            .ToListAsync();
        
        return posts;
    }

    public async Task<IEnumerable<T>> SearchUsers<T>(string query)
    {
        var users = await _users
            .AllAsNoTracking()
            .Where(u => EF.Functions.Like(u.UserName, $"%{query}%"))
            .OrderBy(u => u.UserName)
            .To<T>()
            .ToListAsync();

        return users;
    }
}