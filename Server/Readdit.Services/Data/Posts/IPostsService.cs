using Microsoft.AspNetCore.Http;
using Readdit.Infrastructure.Models;

namespace Readdit.Services.Data.Posts;

public interface IPostsService
{
    Task<CommunityPost?> CreateAsync(
        string authorId,
        string communityId,
        string title,
        string content,
        IEnumerable<string>? tags,
        IFormFile? media);

    Task<bool> DeleteAsync(string userId, string postId);

    Task<CommunityPost?> UpdateAsync(
        string postId,
        string editorId,
        string title,
        string content,
        IFormFile? media);

    Task<T?> GetPostDetailsByIdAsync<T>(string postId, string userId);

    Task<IEnumerable<T>> GetAllByCommunity<T>(string communityId, string userId);
    Task<IEnumerable<T>> GetAllByUser<T>(string userId);
}