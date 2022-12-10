namespace Readdit.Services.Data.Scores;

public interface IPostScoresService
{
    Task<bool> IncreaseForUserAsync(string userId, int amount);
    
    Task<bool> DecreaseForUserAsync(string userId, int amount);
}