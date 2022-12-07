using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.Tags;
using Readdit.Services.External.Cloudinary;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Posts;

public class PostsService : IPostsService
{
    private readonly IDeletableEntityRepository<CommunityPost> _posts;
    private readonly IDeletableEntityRepository<PostVote> _postVotes;
    private readonly IDeletableEntityRepository<PostComment> _postComments;
    private readonly IRepository<PostTag> _postTags;
    private readonly ITagsService _tagsService;

    private readonly IRepository<ApplicationUser> _users;
    private readonly IRepository<Community> _communities;
    private readonly IRepository<UserCommunity> _userCommunities;
    private readonly ICloudinaryService _cloudinaryService;

    public PostsService(
        IDeletableEntityRepository<CommunityPost> posts,
        IRepository<ApplicationUser> users,
        IRepository<Community> communities,
        IRepository<UserCommunity> userCommunities,
        ICloudinaryService cloudinaryService,
        IDeletableEntityRepository<PostVote> postVotes,
        IDeletableEntityRepository<PostComment> postComments,
        IRepository<PostTag> postTags,
        ITagsService tagsService)
    {
        _posts = posts;
        _users = users;
        _communities = communities;
        _userCommunities = userCommunities;
        _cloudinaryService = cloudinaryService;
        _postVotes = postVotes;
        _postComments = postComments;
        _postTags = postTags;
        _tagsService = tagsService;
    }

    public async Task<CommunityPost?> CreateAsync(
        string authorId,
        string communityId,
        string title,
        string content,
        IEnumerable<string>? tags,
        IFormFile? media)
    {
        var user = await _users
            .All()
            .FirstOrDefaultAsync(u => u.Id == authorId);
        if (user is null)
        {
            return null;
        }
            
        var community = await _communities
            .All()
            .FirstOrDefaultAsync(c => c.Id == communityId);
        if (community is null)
        {
            return null;
        }

        var isMember = await _userCommunities
            .AllAsNoTracking()
            .AnyAsync(uc => uc.UserId == authorId
                            && uc.CommunityId == communityId
                            && uc.Status == UserCommunityStatus.Approved);
        if (!isMember || community.Type != CommunityType.Public)
        {
            return null;
        }

        string? postPictureUrl = default;
        if (media is not null)
        {
            postPictureUrl = await _cloudinaryService.UploadAsync(
                media.OpenReadStream(),
                media.FileName,
                media.ContentType);
        }

        var post = new CommunityPost
        {
            Community = community,
            Author = user,
            MediaUrl = postPictureUrl,
            Title = title,
            Content = content
        };
        
        var existingTags = await _tagsService.GetAllByNamesAsync(tags);
        var postTags = existingTags.Select(t => new PostTag
        {
            Tag = t,
            Post = post
        }).ToList();
        
        post.Tags = postTags;
        _posts.Add(post);
        await _posts.SaveChangesAsync();
        
        return post;
    }

    public async Task<bool> DeleteAsync(string userId, string postId)
    {
        var post = await _posts
            .All()
            .FirstOrDefaultAsync(p => p.Id == postId && p.AuthorId == userId);
        if (post is null)
        {
            return false;
        }

        await DeletePostVotes(postId);
        await DeletePostComments(postId);
        await DeletePostTags(postId);

        _posts.Delete(post);
        await _posts.SaveChangesAsync();
        return true;
    }

    public async Task<CommunityPost?> UpdateAsync(
        string postId,
        string editorId,
        string title,
        string content,
        IFormFile? media)
    {
        var post = await _posts
            .All()
            .FirstOrDefaultAsync(p => p.Id == postId);
        if (post is null || post.AuthorId != editorId)
        {
            return null;
        }

        post.Title = title;
        post.Content = content;
        if (media is not null)
        {
            post.MediaUrl = await _cloudinaryService.UploadAsync(
                media.OpenReadStream(),
                media.FileName,
                media.ContentType);
        }
        
        _posts.Update(post);
        await _posts.SaveChangesAsync();
        
        return post;
    }

    private async Task DeletePostVotes(string postId)
    {
        var postVotes = await _postVotes
            .All()
            .Where(pv => pv.PostId == postId)
            .ToListAsync();

        foreach (var postVote in postVotes)
        {
            _postVotes.Delete(postVote);
        }

        await _postVotes.SaveChangesAsync();
    }
    
    private async Task DeletePostTags(string postId)
    {
        var postTags = await _postTags
            .All()
            .Where(pt => pt.PostId == postId)
            .ToListAsync();

        foreach (var postTag in postTags)
        {
            _postTags.Delete(postTag);
        }

        await _postTags.SaveChangesAsync();
    }
    
    private async Task DeletePostComments(string postId)
    {
        var postComments = await _postComments
            .All()
            .Where(pv => pv.PostId == postId)
            .ToListAsync();

        foreach (var postComment in postComments)
        {
            _postComments.Delete(postComment);
        }

        await _postComments.SaveChangesAsync();
    }

    public Task<T?> GetPostDetailsByIdAsync<T>(string postId)
        => _posts
            .AllAsNoTracking()
            .Where(p => p.Id == postId)
            .To<T>()
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<T>> GetAllByCommunity<T>(string communityId)
        => await _posts
            .AllAsNoTracking()
            .Where(p => p.CommunityId == communityId)
            .OrderByDescending(p => p.CreatedOn)
            .ThenByDescending(p => p.Votes.Count)
            .To<T>()
            .ToListAsync();
}