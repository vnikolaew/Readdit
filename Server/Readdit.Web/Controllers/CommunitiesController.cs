using System.Net;
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
        => _communityService = communityService;

    [HttpPost]
    [ProducesResponseType((int) HttpStatusCode.Created, Type = typeof(Community))]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
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
            : CreatedAtAction(nameof(Details), new { communityIdOrName = community.Id }, community);
    }
    
    [HttpGet]
    [Route("{communityIdOrName}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(CommunityDetailsModel))]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    public async Task<IActionResult> Details([FromRoute]string communityIdOrName)
    {
        var community = await _communityService
            .GetCommunityDetailsByIdOrNameAsync<CommunityDetailsModel>(communityIdOrName, User.GetId()!);
        return community.OkOrNotFound();
    }
    
    [HttpPut]
    [Route("{communityId}")]
    [ProducesResponseType((int) HttpStatusCode.Accepted, Type = typeof(Community))]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(
        [FromForm] UpdateCommunityInputModel model,
        string communityId)
    {
        if (communityId != model.CommunityId)
        {
            return BadRequest();
        }
        
        var community = await _communityService.UpdateAsync(
            User.GetId()!,
            model.CommunityId,
            model.Description,
            model.Picture,
            model.Tags,
            model.Type);

        return community is null
            ? BadRequest()
            : AcceptedAtAction(nameof(Details), new { communityId = community.Id }, community);
    }

    [HttpDelete]
    [Route("{communityId}")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Delete([FromRoute] string communityId)
    {
        var success = await _communityService
            .DeleteAsync(User.GetId()!, communityId);

        return success.OkOrBadRequest();
    }
}