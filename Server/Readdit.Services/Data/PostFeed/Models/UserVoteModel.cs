using Readdit.Infrastructure.Models.Enums;

namespace Readdit.Services.Data.PostFeed.Models;

public class UserVoteModel
{
    public string Id { get; set; }
        
    public VoteType Type { get; set; }
}