using Microsoft.AspNetCore.Http;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;

namespace Readdit.Services.Data.Communities;

public interface ICommunityService
{
    Task<Community?> CreateAsync(
        string adminId,
        string name,
        string? description,
        IFormFile? picture,
        IEnumerable<string> tags,
        CommunityType type);

    Task<Community?> UpdateAsync(
        string editorId,
        string communityId,
        string description,
        IFormFile? picture,
        IEnumerable<string> tags,
        CommunityType type);

    Task<bool> DeleteAsync(string userId, string communityId);
    
    Task<T?> GetCommunityDetailsByIdOrNameAsync<T>(string communityIdOrName, string userId);
}