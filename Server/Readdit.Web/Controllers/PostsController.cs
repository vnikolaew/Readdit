using System.Net;
using Microsoft.AspNetCore.Mvc;
using Readdit.Infrastructure.Models;
using Readdit.Services.Data.Posts;
using Readdit.Services.Data.Posts.Models;
using Readdit.Web.Infrastructure.Extensions;

namespace Readdit.Web.Controllers;

public class PostsController : ApiController
{
    private readonly IPostsService _postsService;

    public PostsController(IPostsService postsService)
        => _postsService = postsService;

    [HttpPost]
    [ProducesResponseType((int) HttpStatusCode.Created, Type = typeof(CommunityPost))]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create([FromForm] CreatePostInputModel model)
    {
        var post = await _postsService.CreateAsync(
            User.GetId()!,
            model.CommunityId,
            model.Title,
            model.Content,
            model.Tags,
            model.Media);

        return post is null
            ? BadRequest()
            : CreatedAtAction(nameof(Details), new { postId = post.Id }, post);
    }

    [HttpGet]
    [Route("{postId}")]
    [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(PostDetailsModel))]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    public async Task<IActionResult> Details([FromRoute]string postId)
    {
        var post = await _postsService.GetPostDetailsByIdAsync<PostDetailsModel>(postId);
        return post.OkOrNotFound();
    }

    [HttpDelete]
    [Route("{postId}")]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    [ProducesResponseType((int) HttpStatusCode.NoContent)]
    public async Task<IActionResult> Delete([FromRoute] string postId)
    {
        var success = await _postsService.DeleteAsync(User.GetId()!, postId);
        return success ? BadRequest() : NoContent();
    }
    
    [HttpPut]
    [Route("{postId}")]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(CommunityPost))]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Update(
        [FromForm] UpdatePostInputModel model,
        string postId)
    {
        var post = await _postsService.UpdateAsync(
            model.PostId,
            User.GetId()!,
            model.Title,
            model.Content,
            model.Media);
        
        return post is null
            ? BadRequest()
            : AcceptedAtAction(nameof(Details), new { postId = post.Id }, post);
    }

    [HttpGet]
    [Route("community/{communityId}")]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(IEnumerable<CommunityPostModel>))]
    public async Task<IActionResult> AllByCommunity(
        [FromRoute] string communityId)
    {
        var postsByCommunity = await _postsService
            .GetAllByCommunity<CommunityPostModel>(communityId, User.GetId()!);
        return Ok(postsByCommunity);
    }
}