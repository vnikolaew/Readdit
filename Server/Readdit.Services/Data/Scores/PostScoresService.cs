using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;

namespace Readdit.Services.Data.Scores;

public class PostScoresService : IPostScoresService
{
    private readonly IRepository<UserScore> _userScores;

    public PostScoresService(IRepository<UserScore> userScores)
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

        userScore.PostsScore += amount;
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

        userScore.PostsScore -= amount;
        return true;
    }
}