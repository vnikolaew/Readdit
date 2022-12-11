using System.Net;
using Microsoft.AspNetCore.Mvc;
using Readdit.Services.Data.Comments;
using Readdit.Services.Data.Comments.Models;
using Readdit.Web.Infrastructure.Extensions;

namespace Readdit.Web.Controllers;

public class CommentsController : ApiController
{
    private readonly ICommentsService _commentsService;

    public CommentsController(ICommentsService commentsService)
        => _commentsService = commentsService;

    [HttpPost]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create([FromForm] CreateCommentInputModel commentInputModel)
    {
        var comment = await _commentsService.CreateAsync(
            User.GetId()!,
            commentInputModel.PostId,
            commentInputModel.Content);
        
        return comment.OkOrBadRequest();
    }
    
    [HttpPut]
    [Route("{commentId}")]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    [ProducesResponseType((int) HttpStatusCode.Accepted)]
    public async Task<IActionResult> Update(
        [FromRoute] string commentId,
        UpdateCommentInputModel commentInputModel)
    {
        if (commentId != commentInputModel.CommentId)
        {
            return BadRequest();
        }
        
        var comment = await _commentsService.UpdateAsync(
            User.GetId()!,
            commentId,
            commentInputModel.Content);
        
        return comment is null
            ? BadRequest()
            : AcceptedAtAction(nameof(Create), new { commentId = comment.Id }, comment);
    }

    [HttpDelete]
    [Route("{commentId}")]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    public async Task<IActionResult> Delete([FromRoute] string commentId)
    {
        var success = await _commentsService
            .DeleteAsync(User.GetId()!, commentId);

        return success.OkOrBadRequest();
    }
}