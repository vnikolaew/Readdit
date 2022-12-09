using Microsoft.AspNetCore.Mvc;
using Readdit.Services.Data.PostVotes;
using Readdit.Web.Infrastructure.Extensions;

namespace Readdit.Web.Controllers;

public class PostVotesController : ApiController
{
    private readonly IPostVotesService _postVotesService;

    public PostVotesController(IPostVotesService postVotesService)
        => _postVotesService = postVotesService;

    [HttpPost]
    [Route("up/{postId}")]
    public async Task<IActionResult> UpVote([FromRoute] string postId)
    {
        var postVote = await _postVotesService.UpVoteAsync(User.GetId()!, postId);
        return postVote is null
            ? BadRequest()
            : Ok();
    }
    
    [HttpPost]
    [Route("down/{postId}")]
    public async Task<IActionResult> DownVote([FromRoute] string postId)
    {
        var postVote = await _postVotesService.DownVoteAsync(User.GetId()!, postId);
        return postVote is null
            ? BadRequest()
            : Ok();
    }
    
    [HttpDelete]
    [Route("up/{postId}")]
    public async Task<IActionResult> DeleteUpVote([FromRoute] string postId)
    {
        var success = await _postVotesService.RemoveUpVoteAsync(User.GetId()!, postId);
        return success.OkOrBadRequest();
    }

    [HttpDelete]
    [Route("down/{postId}")]
    public async Task<IActionResult> DeleteDownVote([FromRoute] string postId)
    {
        var success = await _postVotesService.RemoveDownVoteAsync(User.GetId()!, postId);
        return success.OkOrBadRequest();
    }
}