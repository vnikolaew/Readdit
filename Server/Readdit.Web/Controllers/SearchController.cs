using System.Net;
using Microsoft.AspNetCore.Mvc;
using Readdit.Services.Data.PostFeed.Enums;
using Readdit.Services.Data.Search;
using Readdit.Services.Data.Search.Models;

namespace Readdit.Web.Controllers;

public class SearchController : ApiController
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
        => _searchService = searchService;

    [HttpGet]
    [Route("users")]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(IEnumerable<UserSearchModel>))]
    public async Task<IActionResult> SearchUsers([FromQuery] string query)
    {
        var users = await _searchService
            .SearchUsers<UserSearchModel>(query);
        return Ok(users);
    }
    
    [HttpGet]
    [Route("communities")]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(IEnumerable<CommunitySearchModel>))]
    public async Task<IActionResult> SearchCommunities([FromQuery] string query)
    {
        var communities = await _searchService
            .SearchCommunities<CommunitySearchModel>(query);
        return Ok(communities);
    }
    
    [HttpGet]
    [Route("posts")]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(IEnumerable<PostSearchModel>))]
    public async Task<IActionResult> SearchPosts(
        [FromQuery] string query, [FromQuery] TimeRange range)
    {
        var posts = await _searchService
            .SearchPosts<PostSearchModel>(query, range);
        return Ok(posts);
    }
}