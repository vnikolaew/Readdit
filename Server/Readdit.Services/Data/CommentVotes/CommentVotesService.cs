using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;

namespace Readdit.Services.Data.CommentVotes;

public class CommentVotesService : ICommentVotesService
{
    private readonly IRepository<CommentVote> _commentVotes;

    public CommentVotesService(IRepository<CommentVote> commentVotes)
    {
        _commentVotes = commentVotes;
    }

    public async Task<CommentVote> UpVoteAsync(string userId, string commentId)
    {
        var commentVote = await _commentVotes
            .All()
            .FirstOrDefaultAsync(cv =>cv.UserId == userId && cv.CommentId == commentId);

        if (commentVote is null)
        {
            commentVote = new CommentVote
            {
                UserId = userId,
                CommentId = commentId,
                Type = VoteType.Up
            };
            
            _commentVotes.Add(commentVote);
            await _commentVotes.SaveChangesAsync();

            return commentVote;
        }

        if (commentVote.Type == VoteType.Up)
        {
            return commentVote;
        }
        
        commentVote.Type = VoteType.Up;
        _commentVotes.Update(commentVote);
        await _commentVotes.SaveChangesAsync();

        return commentVote;
    }

    public async Task<CommentVote> DownVoteAsync(string userId, string commentId)
    {
        var commentVote = await _commentVotes
            .All()
            .FirstOrDefaultAsync(cv =>cv.UserId == userId && cv.CommentId == commentId);

        if (commentVote is null)
        {
            commentVote = new CommentVote
            {
                UserId = userId,
                CommentId = commentId,
                Type = VoteType.Down
            };
            
            _commentVotes.Add(commentVote);
            await _commentVotes.SaveChangesAsync();

            return commentVote;
        }

        if (commentVote.Type == VoteType.Down)
        {
            return commentVote;
        }
        
        commentVote.Type = VoteType.Down;
        _commentVotes.Update(commentVote);
        
        await _commentVotes.SaveChangesAsync();
        return commentVote;
    }
}