using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.Common;

namespace Readdit.Services.Data.CommentVotes;

public class CommentVotesService : ICommentVotesService
{
    private readonly IRepository<CommentVote> _commentVotes;
    private readonly IRepository<PostComment> _comments;
    private readonly IUnitOfWork _unitOfWork;

    public CommentVotesService(
        IRepository<CommentVote> commentVotes,
        IRepository<PostComment> comments,
        IUnitOfWork unitOfWork)
    {
        _commentVotes = commentVotes;
        _comments = comments;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<CommentVote?> UpVoteAsync(string userId, string commentId)
    {
        var commentVote = await _commentVotes
            .All()
            .FirstOrDefaultAsync(cv =>cv.UserId == userId && cv.CommentId == commentId);

        var comment = await _comments
            .All()
            .FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment is null || commentVote?.Type == VoteType.Up)
        {
            return null;
        }

        if (commentVote is null)
        {
            commentVote = new CommentVote
            {
                UserId = userId,
                Comment = comment,
                Type = VoteType.Up
            };

            comment.VoteScore++;
            _commentVotes.Add(commentVote);
            
            await _unitOfWork.SaveChangesAsync();
            return commentVote;
        }

        
        commentVote.Type = VoteType.Up;
        comment.VoteScore += 2;
        
        _commentVotes.Update(commentVote);
        await _unitOfWork.SaveChangesAsync();

        return commentVote;
    }

    public async Task<bool> RemoveUpVoteAsync(string userId, string commentId)
    {
        var commentVote = await _commentVotes
            .All()
            .FirstOrDefaultAsync(cv => cv.UserId == userId
                                       && cv.CommentId == commentId
                                       && cv.Type == VoteType.Down);
        
        var comment = await _comments
            .All()
            .FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment is null || commentVote is null)
        {
            return false;
        }

        comment.VoteScore--;
        _commentVotes.Delete(commentVote);

        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<CommentVote?> DownVoteAsync(string userId, string commentId)
    {
        var commentVote = await _commentVotes
            .All()
            .FirstOrDefaultAsync(cv =>cv.UserId == userId && cv.CommentId == commentId);
        
        var comment = await _comments
            .All()
            .FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment is null || commentVote?.Type == VoteType.Down)
        {
            return null;
        }

        if (commentVote is null)
        {
            commentVote = new CommentVote
            {
                UserId = userId,
                Comment = comment,
                Type = VoteType.Down
            };

            comment.VoteScore--;
            _commentVotes.Add(commentVote);
            
            await _unitOfWork.SaveChangesAsync();
            return commentVote;
        }

        
        commentVote.Type = VoteType.Down;
        comment.VoteScore -= 2;   
        
        _commentVotes.Update(commentVote);
        await _unitOfWork.SaveChangesAsync();
        
        return commentVote;
    }

    public async Task<bool> RemoveDownVoteAsync(string userId, string commentId)
    {
        var commentVote = await _commentVotes
            .All()
            .FirstOrDefaultAsync(cv =>cv.UserId == userId
                                      && cv.CommentId == commentId
                                      && cv.Type == VoteType.Down);
        
        var comment = await _comments
            .All()
            .FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment is null || commentVote is null)
        {
            return false;
        }

        comment.VoteScore++;
        _commentVotes.Delete(commentVote);

        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}