using Microsoft.AspNetCore.Mvc;
using Readdit.Infrastructure.Models;
using Readdit.Services.Data.Communities;
using Readdit.Services.Data.Communities.Models;
using Readdit.Web.Infrastructure.Extensions;

namespace Readdit.Web.Controllers;

public class CommunitiesController : ApiController
{
    private readonly ICommunityService _communityService;

    public CommunitiesController(ICommunityService communityService)
    {
        _communityService = communityService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateCommunityInputModel model)
    {
        var community = await _communityService.CreateAsync(
            User.GetId()!,
            model.Name,
            model.Description,
            model.Picture,
            model.Tags,
            model.Type);

        return community is null
            ? BadRequest()
            : CreatedAtAction(nameof(Details), new { communityId = community.Id }, community);
    }
    
    [HttpGet]
    [Route("{communityId}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Details([FromRoute]string communityId)
    {
        var post = await _communityService.GetCommunityDetailsByIdAsync<CommunityPost>(communityId);
        return post.OkOrNotFound();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdateCommunityInputModel model)
    {
        var community = await _communityService.UpdateAsync(
            User.GetId()!,
            model.CommunityId,
            model.Description,
            model.Picture,
            model.Tags,
            model.Type);

        return community is null
            ? BadRequest()
            : Ok(community);
    }

    [HttpDelete]
    [Route("{communityId}")]
    public async Task<IActionResult> Delete([FromRoute] string communityId)
    {
        var success = await _communityService.DeleteAsync(User.GetId()!, communityId);
        return success ? Ok() : BadRequest();
    }
}