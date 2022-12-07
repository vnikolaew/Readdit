using Readdit.Infrastructure.Models;

namespace Readdit.Services.Data.Comments;

public interface ICommentsService
{
    Task<PostComment?> CreateAsync(
        string authorId,
        string postId,
        string content
    );
    
    Task<PostComment?> UpdateAsync(
        string authorId,
        string commentId,
        string content
    );
    
    Task<bool> DeleteAsync(
        string authorId,
        string commentId
    );
}