using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;

namespace Readdit.Services.Data.PostVotes;

public class PostVotesService : IPostVotesService
{
    private readonly IRepository<PostVote> _postVotes;

    public PostVotesService(IRepository<PostVote> postVotes)
    {
        _postVotes = postVotes;
    }

    public async Task<PostVote> UpVoteAsync(string userId, string postId)
    {
        var postVote = await _postVotes
            .All()
            .FirstOrDefaultAsync(pv => pv.UserId == userId && pv.PostId == postId);

        if (postVote is null)
        {
            postVote = new PostVote
            {
                UserId = userId,
                PostId = postId,
                Type = VoteType.Up
            };
            
            _postVotes.Add(postVote);
            await _postVotes.SaveChangesAsync();

            return postVote;
        }

        if (postVote.Type == VoteType.Up)
        {
            return postVote;
        }
        
        postVote.Type = VoteType.Up;
        _postVotes.Update(postVote);
        await _postVotes.SaveChangesAsync();

        return postVote;
    }

    public async Task<PostVote> DownVoteAsync(string userId, string postId)
    {
        var postVote = await _postVotes
            .All()
            .FirstOrDefaultAsync(pv => pv.UserId == userId && pv.PostId == postId);

        if (postVote is null)
        {
            postVote = new PostVote
            {
                UserId = userId,
                PostId = postId,
                Type = VoteType.Down
            };
            
            _postVotes.Add(postVote);
            await _postVotes.SaveChangesAsync();

            return postVote;
        }

        if (postVote.Type == VoteType.Down)
        {
            return postVote;
        }
        
        postVote.Type = VoteType.Down;
        _postVotes.Update(postVote);
        
        await _postVotes.SaveChangesAsync();
        return postVote;
    }
}