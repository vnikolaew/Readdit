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
    {
        _postsService = postsService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePostInputModel model)
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
    public async Task<IActionResult> Details([FromRoute]string postId)
    {
        var post = await _postsService.GetPostDetailsByIdAsync<CommunityPost>(postId);
        return post.OkOrNotFound();
    }

    [HttpDelete]
    [Route("{postId}")]
    public async Task<IActionResult> Delete([FromRoute] string postId)
    {
        var success = await _postsService.DeleteAsync(User.GetId()!, postId);
        return success ? BadRequest() : NoContent();
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdatePostInputModel model)
    {
        var post = await _postsService.UpdateAsync(
            model.PostId,
            User.GetId()!,
            model.Title,
            model.Content,
            model.Media);
        
        return post is null ? BadRequest() : Ok(post);
    }

    [HttpGet]
    [Route("community/{communityId}")]
    public async Task<IActionResult> AllByCommunity([FromRoute] string communityId)
    {
        var postsByCommunity = await _postsService
            .GetAllByCommunity<CommunityPostServiceModel>(communityId);
        return Ok(postsByCommunity);
    }
}