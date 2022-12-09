﻿using Microsoft.AspNetCore.Mvc;
using Readdit.Services.Data.CommentVotes;
using Readdit.Web.Infrastructure.Extensions;

namespace Readdit.Web.Controllers;

public class CommentVotesController : ApiController
{
    private readonly ICommentVotesService _commentVotesService;

    public CommentVotesController(ICommentVotesService commentVotesService)
        => _commentVotesService = commentVotesService;
        
    [HttpPost]
    [Route("up/{commentId}")]
    public async Task<IActionResult> UpVote([FromRoute] string commentId)
    {
        var commentVote = await _commentVotesService.UpVoteAsync(
            User.GetId()!, commentId);

        return commentVote.OkOrBadRequest();
    }
        
    [HttpPost]
    [Route("down/{commentId}")]
    public async Task<IActionResult> DownVote([FromRoute] string commentId)
    {
        var commentVote = await _commentVotesService.DownVoteAsync(
            User.GetId()!, commentId);
        
        return commentVote.OkOrBadRequest();
    }
    
    [HttpDelete]
    [Route("up/{commentId}")]
    public async Task<IActionResult> DeleteUpVote([FromRoute] string commentId)
    {
        var success = await _commentVotesService.RemoveUpVoteAsync(
            User.GetId()!, commentId);
        
        return success.OkOrBadRequest();
    }

    [HttpDelete]
    [Route("down/{commentId}")]
    public async Task<IActionResult> DeleteDownVote([FromRoute] string commentId)
    {
        var success = await _commentVotesService.RemoveDownVoteAsync(
            User.GetId()!, commentId);
        
        return success.OkOrBadRequest();
    }
}