namespace Readdit.Services.Data.PostFeed.Enums;

public enum TimeRange : sbyte
{
    Today = 1,
    ThisHour = 2,
    ThisWeek = 3,
    ThisMonth = 4,
    ThisYear = 5,
    AllTime = 6,
}

public static class Extensions
{
    public static DateTime GetStartDate(this TimeRange range) 
        => range switch
        {
            TimeRange.Today => DateTime.UtcNow.AddDays(-1),
            TimeRange.ThisHour => DateTime.UtcNow.AddHours(-1),
            TimeRange.ThisWeek => DateTime.UtcNow.AddDays(-7),
            TimeRange.ThisMonth => DateTime.UtcNow.AddMonths(-1),
            TimeRange.ThisYear => DateTime.UtcNow.AddYears(-1),
            TimeRange.AllTime => DateTime.UnixEpoch,
            _ => DateTime.UtcNow
        };
}