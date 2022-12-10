using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.PostFeed.Enums;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.PostFeed;

public class PostFeedService : IPostFeedService
{
    private readonly IRepository<CommunityPost> _posts;
    private readonly IRepository<UserCommunity> _userCommunities;

    public PostFeedService(
        IRepository<CommunityPost> posts,
        IRepository<UserCommunity> userCommunities)
    {
        _posts = posts;
        _userCommunities = userCommunities;
    }

    public async Task<IEnumerable<T>> GetMostRecentForUserAsync<T>(string userId)
    {
        var communityIds = await _userCommunities
            .AllAsNoTracking()
            .Where(uc => uc.UserId == userId
                         && uc.Community.Type != CommunityType.Private)
            .Select(uc => uc.CommunityId)
            .ToListAsync();

        var posts = await _posts
            .AllAsNoTracking()
            .Where(p => communityIds.Contains(p.CommunityId))
            .OrderByDescending(p => p.CreatedOn)
            .ThenByDescending(p => p.Votes.Count)
            .To<T>()
            .ToListAsync();
        
        return posts;
    }
    
    public async Task<IEnumerable<T>> GetBestVotedForUserAsync<T>(
        string userId,
        TimeRange range)
    {
        var communityIds = await _userCommunities
            .AllAsNoTracking()
            .Where(uc => uc.UserId == userId
                         && uc.Community.Type != CommunityType.Private)
            .Select(uc => uc.CommunityId)
            .ToListAsync();

        var startDate = range.GetStartDate();
        var posts = await _posts
            .AllAsNoTracking()
            .Where(p => communityIds.Contains(p.CommunityId)
                        && p.CreatedOn > startDate)
            .OrderByDescending(p => p.Votes.Count)
            .ThenByDescending(p => p.CreatedOn)
            .To<T>()
            .ToListAsync();
        
        return posts;
    }
}