using Readdit.Infrastructure.Models;

namespace Readdit.Services.Data.UserCommunities;

public interface IUserCommunityService
{
    Task<UserCommunity?> JoinCommunityAsync(string userId, string communityId);
    
    Task<bool> LeaveCommunityAsync(string userId, string communityId);

    Task<UserCommunity?> GetByUserAndCommunity(string userId, string communityId);

    Task<UserCommunity?> ApproveUserAsync(
        string approverId,
        string userId,
        string communityId);

    Task<IEnumerable<T>> GetAllByUser<T>(string userId);
}