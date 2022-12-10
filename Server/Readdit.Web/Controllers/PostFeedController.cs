using Microsoft.AspNetCore.Mvc;
using Readdit.Services.Data.PostFeed;
using Readdit.Services.Data.PostFeed.Enums;
using Readdit.Services.Data.PostFeed.Models;
using Readdit.Web.Infrastructure.Extensions;

namespace Readdit.Web.Controllers;

public class PostFeedController : ApiController
{
    private readonly IPostFeedService _postFeedService;

    public PostFeedController(IPostFeedService postFeedService)
        => _postFeedService = postFeedService;

    [HttpGet]
    [Route("new")]
    public async Task<IActionResult> MostRecentPostsAsync()
    {
        var posts = await _postFeedService
            .GetMostRecentForUserAsync<FeedCommunityPostModel>(User.GetId()!);
        return Ok(posts);
    }
    
    [HttpGet]
    [Route("top")]
    public async Task<IActionResult> BesVotedPostsAsync([FromQuery] TimeRange range)
    {
        var posts = await _postFeedService
            .GetBestVotedForUserAsync<FeedCommunityPostModel>(User.GetId()!, range);
        return Ok(posts);
    }
}