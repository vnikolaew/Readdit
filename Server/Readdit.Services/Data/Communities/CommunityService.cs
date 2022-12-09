using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.Posts;
using Readdit.Services.Data.Tags;
using Readdit.Services.External.Cloudinary;
using Readdit.Services.External.Cloudinary.Models;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Communities;

public class CommunityService : ICommunityService
{
    private readonly IDeletableEntityRepository<Community> _communities;
    private readonly IDeletableEntityRepository<UserCommunity> _userCommunities;
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
        IRepository<CommunityPost> posts,
        IDeletableEntityRepository<UserCommunity> userCommunities)
    {
        _communities = communities;
        _users = users;
        _cloudinaryService = cloudinaryService;
        _tagsService = tagsService;
        _postsService = postsService;
        _posts = posts;
        _userCommunities = userCommunities;
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

        ImageUploadResult? uploadResult = null;
        if (picture is not null)
        {
            uploadResult = await _cloudinaryService.UploadAsync(
                picture.OpenReadStream(),
                picture.FileName,
                picture.ContentType);
        }
        
        var community = new Community
        {
            Admin = admin,
            Name = name,
            Description = description!,
            PictureUrl = uploadResult?.AbsoluteImageUrl ?? string.Empty,
            PicturePublicId = uploadResult?.ImagePublidId ?? string.Empty,
            Type = type
        };
        var userCommunity = new UserCommunity
        {
            User = admin,
            Community = community,
            Status = UserCommunityStatus.Approved
        };

        var existingTags = await _tagsService.GetAllByNamesAsync(tags);
        var communityTags = existingTags.Select(t => new CommunityTag
        {
            Community = community,
            Tag = t
        }).ToList();

        community.Tags = communityTags;
        
        _communities.Add(community);
        _userCommunities.Add(userCommunity);
        
        await _communities.SaveChangesAsync();
        await _userCommunities.SaveChangesAsync();

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

        if (picture is not null)
        {
            await DeleteCommunityPictureIfPresent(community);
            
            var uploadResult = await _cloudinaryService.UploadAsync(
                picture.OpenReadStream(),
                picture.FileName,
                picture.ContentType
            );
            
            community.PictureUrl = uploadResult.AbsoluteImageUrl;
            community.PicturePublicId = uploadResult.ImagePublidId;
        }

        var existingTags = await _tagsService.GetAllByNamesAsync(tags);
        var communityTags = existingTags
            .Select(t => new CommunityTag
            {
                Community = community,
                Tag = t
            })
            .ToList();
        
        community.Tags.Clear();
        
        community.Tags = communityTags;
        community.Description = description;
        community.Type = type;

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
        await DeleteCommunityPictureIfPresent(community);
        await DeletePostsByCommunityId(communityId, user.Id);
        
        await _communities.SaveChangesAsync();
        
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
            .AllAsNoTracking()
            .Where(p => p.CommunityId == communityId)
            .Select(p => new { p.Id })
            .ToListAsync();

        foreach (var postIdInfo in postIds)
        {
            await _postsService.DeleteAsync(userId, postIdInfo.Id);
        }
    }
    
    private async Task DeleteCommunityPictureIfPresent(Community community)
    {
        if (!string.IsNullOrEmpty(community.PictureUrl))
        {
            await _cloudinaryService.DeleteFileAsync(community.PicturePublicId);
        }
    }
}