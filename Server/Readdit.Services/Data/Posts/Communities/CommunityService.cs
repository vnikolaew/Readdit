using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.Posts;
using Readdit.Services.Data.Tags;
using Readdit.Services.External.Cloudinary;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Communities;

public class CommunityService : ICommunityService
{
    private readonly IDeletableEntityRepository<Community> _communities;
    private readonly IRepository<CommunityPost> _posts;
    private readonly IRepository<ApplicationUser> _users;
    
    private readonly ITagsService _tagsService;
    private readonly IPostsService _postsService;
    private readonly ICloudinaryService _cloudinaryService;

    public CommunityService(
        IDeletableEntityRepository<Community> communities,
        IRepository<ApplicationUser> users,
        ICloudinaryService cloudinaryService,
        ITagsService tagsService,
        IPostsService postsService,
        IRepository<CommunityPost> posts)
    {
        _communities = communities;
        _users = users;
        _cloudinaryService = cloudinaryService;
        _tagsService = tagsService;
        _postsService = postsService;
        _posts = posts;
    }

    public async Task<Community?> CreateAsync(
        string adminId,
        string name,
        string? description,
        IFormFile? picture,
        IEnumerable<string> tags,
        CommunityType type)
    {
        var admin = await _users
            .All()
            .FirstOrDefaultAsync(u => u.Id == adminId);
        if (admin is null)
        {
            return null;
        }

        var newPictureUrl = string.Empty;
        if (picture is not null)
        {
            newPictureUrl = await _cloudinaryService.UploadAsync(
                picture.OpenReadStream(),
                picture.FileName,
                picture.ContentType);
        }
        
        var community = new Community
        {
            Admin = admin,
            Name = name,
            Description = description!,
            PictureUrl = newPictureUrl,
            Type = type
        };

        var existingTags = await _tagsService.GetAllByNamesAsync(tags);
        var communityTags = existingTags.Select(t => new CommunityTag
        {
            Community = community,
            Tag = t
        }).ToList();

        community.Tags = communityTags;
        _communities.Add(community);
        await _communities.SaveChangesAsync();

        return community;
    }

    public async Task<Community?> UpdateAsync(
        string editorId,
        string communityId,
        string description,
        IFormFile? picture,
        IEnumerable<string> tags,
        CommunityType type)
    {
        var user = await _users
            .AllAsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == editorId);
        if (user is null)
        {
            return null;
        }

        var community = await _communities
            .All()
            .Include(c => c.Tags)
            .FirstOrDefaultAsync(c => c.Id == communityId);
        if (community is null || community.AdminId != user.Id)
        {
            return community;
        }

        var newPictureUrl = community.PictureUrl;
        if (picture is not null)
        {
            newPictureUrl = await _cloudinaryService.UploadAsync(
                picture.OpenReadStream(),
                picture.FileName,
                picture.ContentType
            );
        }

        var existingTags = await _tagsService.GetAllByNamesAsync(tags);

        community.Description = description;
        community.PictureUrl = newPictureUrl;
        community.Type = type;
        
        var communityTags = existingTags
            .Select(t => new CommunityTag
            {
                Community = community,
                Tag = t
            })
            .ToList();
        
        community.Tags.Clear();
        community.Tags = communityTags;

        _communities.Update(community);
        await _communities.SaveChangesAsync();
        
        return community;
    }

    public async Task<bool> DeleteAsync(string userId, string communityId)
    {
        var user = await _users
            .AllAsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            return false;
        }

        var community = await _communities
            .All()
            .FirstOrDefaultAsync(c => c.Id == communityId);
        if (community is null || community.AdminId != user.Id)
        {
            return false;
        }
        
        _communities.Delete(community);
        await _communities.SaveChangesAsync();
        
        await DeletePostsByCommunityId(communityId, user.Id);
        return true;
    }

    public Task<T?> GetCommunityDetailsByIdAsync<T>(string communityId)
        => _communities
            .AllAsNoTracking()
            .Where(c => c.Id == communityId)
            .To<T>()
            .FirstOrDefaultAsync();

    private async Task DeletePostsByCommunityId(string communityId, string userId)
    {
        var postIds = await _posts
            .All()
            .Where(p => p.CommunityId == communityId)
            .Select(p => p.Id)
            .ToListAsync();

        foreach (var postId in postIds)
        {
            await _postsService.DeleteAsync(userId, postId);
        }
    }
}