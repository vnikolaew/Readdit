using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;

namespace Readdit.Services.Data.UserCommunities;

public class UserCommunityService : IUserCommunityService
{
    private readonly IDeletableEntityRepository<UserCommunity> _userCommunities;
    private readonly IRepository<ApplicationUser> _users;
    private readonly IRepository<Community> _communities;

    public UserCommunityService(
        IDeletableEntityRepository<UserCommunity> userCommunities,
        IRepository<ApplicationUser> users,
        IRepository<Community> communities)
    {
        _userCommunities = userCommunities;
        _users = users;
        _communities = communities;
    }

    public async Task<UserCommunity?> JoinCommunityAsync(string userId, string communityId)
    {
        var user = await _users.All()
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            return null;
        }

        var community = await _communities.All()
            .FirstOrDefaultAsync(c => c.Id == communityId);
        if (community is null)
        {
            return null;
        }

        var existingUserCommunity = await _userCommunities
            .AllAsNoTracking()
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CommunityId == communityId);
        if (existingUserCommunity is not null)
        {
            return null;
        }

        var userCommunity = new UserCommunity
        {
            Community = community,
            User = user,
            Status = community.Type == CommunityType.Public
                ? UserCommunityStatus.Approved
                : UserCommunityStatus.Pending
        };

        _userCommunities.Add(userCommunity);
        await _userCommunities.SaveChangesAsync();

        return userCommunity;
    }

    public async Task<bool> LeaveCommunityAsync(string userId, string communityId)
    {
        var existingUserCommunity = await _userCommunities
            .All()
            .FirstOrDefaultAsync(uc => uc.UserId == userId
                                       && uc.CommunityId == communityId);
        if (existingUserCommunity is null)
        {
            return false;
        }

        _userCommunities.Delete(existingUserCommunity);
        await _userCommunities.SaveChangesAsync();
        return true;
    }

    public async Task<UserCommunity?> ApproveUserAsync(
        string approverId,
        string userId,
        string communityId)
    {
        var community = await _communities
            .All()
            .Include(c => c.Admin)
            .FirstOrDefaultAsync(c => c.Id == communityId
                                      && c.AdminId == approverId);
        if (community is null || community.AdminId != approverId)
        {
            return null;
        }

        var existingUserCommunity = await _userCommunities
            .All()
            .FirstOrDefaultAsync(uc => uc.CommunityId == communityId
                                       && uc.UserId == userId);
        if (existingUserCommunity is null || existingUserCommunity.Status != UserCommunityStatus.Pending)
        {
            return null;
        }

        existingUserCommunity.Status = UserCommunityStatus.Approved;
        
        _userCommunities.Update(existingUserCommunity);
        await _userCommunities.SaveChangesAsync();
        
        return existingUserCommunity;
    }
}