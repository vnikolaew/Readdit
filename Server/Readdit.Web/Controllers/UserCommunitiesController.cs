using System.Net;
using Microsoft.AspNetCore.Mvc;
using Readdit.Infrastructure.Models;
using Readdit.Services.Data.UserCommunities;
using Readdit.Web.Infrastructure.Extensions;

namespace Readdit.Web.Controllers;

public class UserCommunitiesController : ApiController
{
    private readonly IUserCommunityService _userCommunities;

    public UserCommunitiesController(IUserCommunityService userCommunities)
        => _userCommunities = userCommunities;

    [HttpPost]
    [Route("join/{communityId}")]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(UserCommunity))]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> JoinCommunity([FromRoute]string communityId)
    {
        var userCommunity = await _userCommunities
            .JoinCommunityAsync(User.GetId()!, communityId);
        return userCommunity.OkOrBadRequest();
    }
    
    [HttpPost]
    [Route("leave/{communityId}")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> LeaveCommunity([FromRoute]string communityId)
    {
        var success = await _userCommunities
            .LeaveCommunityAsync(User.GetId()!, communityId);
        return success.OkOrBadRequest();
    }
    
    [HttpPost]
    [Route("approve/{communityId}/{userId}")]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(UserCommunity))]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ApproveCommunity(
        [FromRoute]string communityId, [FromRoute]string userId)
    {
        var userCommunity = await _userCommunities
            .ApproveUserAsync(User.GetId()!, userId, communityId);
        return userCommunity.OkOrBadRequest();
    }
    
    [HttpGet]
    [Route("{communityId}")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetRelationship(
        [FromRoute]string communityId)
    {
        var userCommunity = await _userCommunities
            .GetByUserAndCommunity(User.GetId()!, communityId);

        return userCommunity is null
            ? NotFound()
            : Ok(new { userCommunity.Status, userCommunity.CreatedOn });
    }
}