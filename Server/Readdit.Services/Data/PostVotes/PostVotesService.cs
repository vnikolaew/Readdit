using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.Common.UnitOfWork;
using Readdit.Services.Data.Scores;

namespace Readdit.Services.Data.PostVotes;

public class PostVotesService : IPostVotesService
{
    private readonly IRepository<PostVote> _postVotes;
    private readonly IRepository<CommunityPost> _posts;
    private readonly IPostScoresService _postScores;
    private readonly IUnitOfWork _unitOfWork;

    public PostVotesService(
        IRepository<PostVote> postVotes,
        IRepository<CommunityPost> posts,
        IUnitOfWork unitOfWork,
        IPostScoresService postScores)
    {
        _postVotes = postVotes;
        _posts = posts;
        _unitOfWork = unitOfWork;
        _postScores = postScores;
    }

    public async Task<PostVote?> UpVoteAsync(string userId, string postId)
    {
        var postVote = await _postVotes
            .All()
            .FirstOrDefaultAsync(pv => pv.UserId == userId && pv.PostId == postId);
        
        var post = await _posts
            .All()
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post is null || postVote?.Type == VoteType.Up)
        {
            return null;
        }

        if (postVote is null)
        {
            postVote = new PostVote
            {
                UserId = userId,
                PostId = postId,
                Type = VoteType.Up
            };
            
            post.VoteScore++;
            _postVotes.Add(postVote);

            await _postScores.IncreaseForUserAsync(post.AuthorId, 1);
            await _unitOfWork.SaveChangesAsync();

            return postVote;
        }
        
        postVote.Type = VoteType.Up;
        post.VoteScore += 2;
        
        _postVotes.Update(postVote);
        await _postScores.IncreaseForUserAsync(post.AuthorId, 2);
        await _unitOfWork.SaveChangesAsync();

        return postVote;
    }

    public async Task<bool> RemoveUpVoteAsync(string userId, string postId)
    {
        var postVote = await _postVotes
            .All()
            .FirstOrDefaultAsync(pv => pv.UserId == userId
                                       && pv.PostId == postId
                                       && pv.Type == VoteType.Up);
        var post = await _posts
            .All()
            .FirstOrDefaultAsync(p => p.Id == postId);
        
        if (post is null || postVote is null)
        {
            return false;
        }

        post.VoteScore--;
        
        _postVotes.Delete(postVote);
        await _postScores.DecreaseForUserAsync(post.AuthorId, 1);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }

    public async Task<PostVote?> DownVoteAsync(string userId, string postId)
    {
        var postVote = await _postVotes
            .All()
            .FirstOrDefaultAsync(pv => pv.UserId == userId && pv.PostId == postId);
        
        var post = await _posts
            .All()
            .FirstOrDefaultAsync(p => p.Id == postId);
        
        if (post is null || postVote?.Type == VoteType.Down)
        {
            return null;
        }

        if (postVote is null)
        {
            postVote = new PostVote
            {
                UserId = userId,
                PostId = postId,
                Type = VoteType.Down
            };

            post.VoteScore--;
            _postVotes.Add(postVote);

            await _postScores.DecreaseForUserAsync(post.AuthorId, 1);
            await _unitOfWork.SaveChangesAsync();
            
            return postVote;
        }
        
        postVote.Type = VoteType.Down;
        post.VoteScore -= 2;
        
        _postVotes.Update(postVote);
        await _postScores.DecreaseForUserAsync(post.AuthorId, 2);
        await _unitOfWork.SaveChangesAsync();
        
        return postVote;
    }

    public async Task<bool> RemoveDownVoteAsync(string userId, string postId)
    {
        var postVote = await _postVotes
            .All()
            .FirstOrDefaultAsync(pv => pv.UserId == userId
                                       && pv.PostId == postId
                                       && pv.Type == VoteType.Down);
        var post = await _posts
            .All()
            .FirstOrDefaultAsync(p => p.Id == postId);
        
        if (post is null || postVote is null)
        {
            return false;
        }

        post.VoteScore++;
        
        _postVotes.Delete(postVote);
        await _postScores.IncreaseForUserAsync(post.AuthorId, 1);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}