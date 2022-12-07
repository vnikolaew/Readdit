using Readdit.Infrastructure.Models;

namespace Readdit.Services.Data.CommentVotes;

public interface ICommentVotesService
{
    Task<CommentVote> UpVoteAsync(string userId, string commentId); 
    
    Task<CommentVote> DownVoteAsync(string userId, string commentId);
}