using Microsoft.AspNetCore.Mvc;
using Readdit.Infrastructure.Models;
using Readdit.Services.Data.PostFeed.Enums;
using Readdit.Services.Data.Search;

namespace Readdit.Web.Controllers;

public class SearchController : ApiController
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
        => _searchService = searchService;

    [HttpGet]
    [Route("users")]
    public async Task<IActionResult> SearchUsers([FromQuery] string query)
    {
        var users = await _searchService
            .SearchUsers<ApplicationUser>(query);
        
        return Ok(users);
    }
    
    [HttpGet]
    [Route("communities")]
    public async Task<IActionResult> SearchCommunities([FromQuery] string query)
    {
        var communities = await _searchService
            .SearchCommunities<Community>(query);
        
        return Ok(communities);
    }
    
    [HttpGet]
    [Route("posts")]
    public async Task<IActionResult> SearchPosts(
        [FromQuery] string query, [FromQuery] TimeRange range)
    {
        var posts = await _searchService
            .SearchPosts<CommunityPost>(query, range);
        
        return Ok(posts);
    }
}