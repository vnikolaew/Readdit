using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.Scores;
using Readdit.Services.Data.Tags;
using Readdit.Services.External.Cloudinary;
using Readdit.Services.Mapping;
using ImageUploadResult = Readdit.Services.External.Cloudinary.Models.ImageUploadResult;

namespace Readdit.Services.Data.Posts;

public class PostsService : IPostsService
{
    private readonly IDeletableEntityRepository<CommunityPost> _posts;
    private readonly IDeletableEntityRepository<PostVote> _postVotes;
    private readonly IDeletableEntityRepository<PostComment> _postComments;
    private readonly IDeletableEntityRepository<CommentVote> _commentVotes;
    private readonly IRepository<PostTag> _postTags;
    private readonly ITagsService _tagsService;

    private readonly IRepository<ApplicationUser> _users;
    private readonly IRepository<Community> _communities;
    private readonly IRepository<UserCommunity> _userCommunities;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IPostScoresService _postScores;
    private readonly ICommentScoreService _commentScores;

    public PostsService(
        IDeletableEntityRepository<CommunityPost> posts,
        IRepository<ApplicationUser> users,
        IRepository<Community> communities,
        IRepository<UserCommunity> userCommunities,
        ICloudinaryService cloudinaryService,
        IDeletableEntityRepository<PostVote> postVotes,
        IDeletableEntityRepository<PostComment> postComments,
        IRepository<PostTag> postTags,
        ITagsService tagsService,
        IPostScoresService postScores,
        IDeletableEntityRepository<CommentVote> commentVotes,
        ICommentScoreService commentScores)
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
        _postScores = postScores;
        _commentVotes = commentVotes;
        _commentScores = commentScores;
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

        ImageUploadResult? uploadResult = null;
        if (media is not null)
        {
            uploadResult = await _cloudinaryService.UploadAsync(
                media.OpenReadStream(),
                media.FileName,
                media.ContentType);
        }

        var post = new CommunityPost
        {
            Community = community,
            Author = user,
            MediaUrl = uploadResult?.AbsoluteImageUrl ?? string.Empty,
            MediaPublicId = uploadResult?.ImagePublidId ?? string.Empty,
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

        var deleteTasks = new[]
        {
            DeletePostVotes(post),
            DeletePostComments(post),
            DeletePostTags(postId),
            DeletePostMediaIfPresent(post)
        };
        
        _posts.Delete(post);
        await Task.WhenAll(deleteTasks);
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
            await DeletePostMediaIfPresent(post);
            
            var uploadResult = await _cloudinaryService.UploadAsync(
                media.OpenReadStream(),
                media.FileName,
                media.ContentType);

            post.MediaUrl = uploadResult.AbsoluteImageUrl;
            post.MediaPublicId = uploadResult.ImagePublidId;
        }
        
        _posts.Update(post);
        await _posts.SaveChangesAsync();
        
        return post;
    }

    private async Task DeletePostVotes(CommunityPost post)
    {
        var postVotes = await _postVotes
            .All()
            .Where(pv => pv.PostId == post.Id)
            .ToListAsync();

        foreach (var postVote in postVotes)
        {
            _postVotes.Delete(postVote);
        }
        
        await _postScores.DecreaseForUserAsync(post.AuthorId, postVotes.Count);
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
    }
    
    private async Task DeletePostComments(CommunityPost post)
    {
        var postComments = await _postComments
            .All()
            .Include(c => c.Votes)
            .Where(c => c.PostId == post.Id)
            .ToListAsync();

        foreach (var postComment in postComments)
        {
            _postComments.Delete(postComment);
            foreach (var commentVote in postComment.Votes)
            {
                _commentVotes.Delete(commentVote);
            }

            await _commentScores.DecreaseForUserAsync(postComment.AuthorId, postComment.Votes.Count);
        }

        await _postScores.DecreaseForUserAsync(post.AuthorId, postComments.Count);
    }

    public Task<T?> GetPostDetailsByIdAsync<T>(string postId, string userId)
        => _posts
            .AllAsNoTracking()
            .Where(p => p.Id == postId)
            .To<T>(new { currentUserId = userId })
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<T>> GetAllByCommunity<T>(string communityId, string userId)
        => await _posts
            .AllAsNoTracking()
            .Where(p => p.CommunityId == communityId)
            .OrderByDescending(p => p.CreatedOn)
            .ThenByDescending(p => p.Votes.Count)
            .To<T>(new { currentUserId = userId })
            .ToListAsync();

    public async Task<IEnumerable<T>> GetAllByUser<T>(string userId)
        => await _posts
            .AllAsNoTracking()
            .Where(p => p.AuthorId == userId)
            .OrderByDescending(p => p.CreatedOn)
            .ThenByDescending(p => p.Votes.Count)
            .To<T>(new { currentUserId = userId })
            .ToListAsync();

    private async Task DeletePostMediaIfPresent(CommunityPost post)
    {
        if (!string.IsNullOrEmpty(post.MediaUrl))
        {
            await _cloudinaryService.DeleteFileAsync(post.MediaPublicId);
        }
    }
}