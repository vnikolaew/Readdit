using Readdit.Infrastructure.Models;

namespace Readdit.Services.Data.PostVotes;

public interface IPostVotesService
{
    Task<PostVote?> UpVoteAsync(string userId, string postId); 
    Task<bool> RemoveUpVoteAsync(string userId, string postId); 
    
    Task<PostVote?> DownVoteAsync(string userId, string postId);
    
    Task<bool> RemoveDownVoteAsync(string userId, string postId); 
}