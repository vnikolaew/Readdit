namespace Readdit.Services.Data.Scores;

public interface ICommentScoreService
{
    Task<bool> IncreaseForUserAsync(string userId, int amount);
    
    Task<bool> DecreaseForUserAsync(string userId, int amount);
}