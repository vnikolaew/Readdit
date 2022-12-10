using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;

namespace Readdit.Services.Data.Scores;

public class CommentScoreService : ICommentScoreService
{
    private readonly IRepository<UserScore> _userScores;

    public CommentScoreService(IRepository<UserScore> userScores)
        => _userScores = userScores;

    public async Task<bool> IncreaseForUserAsync(string userId, int amount)
    {
        var userScore = await _userScores
            .All()
            .FirstOrDefaultAsync(us => us.Id == userId);
        if (userScore is null)
        {
            return false;
        }

        userScore.CommentsScore += amount;
        return true;
    }

    public async Task<bool> DecreaseForUserAsync(string userId, int amount)
    {
        var userScore = await _userScores
            .All()
            .FirstOrDefaultAsync(us => us.Id == userId);
        if (userScore is null)
        {
            return false;
        }

        userScore.CommentsScore -= amount;
        return true;
    }
}