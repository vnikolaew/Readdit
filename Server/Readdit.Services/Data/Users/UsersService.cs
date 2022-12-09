using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.External.Cloudinary;
using Readdit.Services.Mapping;

namespace Readdit.Services.Data.Users;

public class UsersService : IUsersService
{
    private readonly IRepository<ApplicationUser> _users;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IRepository<Country> _countries;

    public UsersService(
        IRepository<ApplicationUser> users,
        ICloudinaryService cloudinaryService,
        IRepository<Country> countries)
    {
        _users = users;
        _cloudinaryService = cloudinaryService;
        _countries = countries;
    }

    public Task<ApplicationUser?> GetUserByIdAsync(string id)
        => _users.All().FirstOrDefaultAsync(u => u.Id == id);

    public Task<T?> GetUserInfoAsync<T>(string id)
        => _users
            .All()
            .Where(u => u.Id == id)
            .To<T>()
            .FirstOrDefaultAsync();

    public async Task<ApplicationUser?> UpdateUserProfileAsync(
        string userId, string firstName,
        string lastName, string gender, string country,
        string aboutContent, IFormFile? profilePicture)
    {
        var user = await GetUserByIdWithProfileAsync(userId);

        if (user is null)
        {
            return null;
        }

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Gender = Enum.Parse<Gender>(gender);
        user.Profile.AboutContent = aboutContent;

        var existingCountry = await _countries
            .All()
            .FirstOrDefaultAsync(c => c.Name == country);
        if (existingCountry is not null)
        {
            user.Country = existingCountry;
        }

        if (profilePicture != null)
        {
            await DeleteUserPictureIfPresent(user);
                
            var uploadResult = await _cloudinaryService.UploadAsync(
                profilePicture.OpenReadStream(),
                profilePicture.FileName,
                profilePicture.ContentType);
            
            user.Profile.ProfilePictureUrl = uploadResult.AbsoluteImageUrl;
            user.Profile.ProfilePicturePublicId = uploadResult.ImagePublidId;
        }

        await _users.SaveChangesAsync();
        return user;
    }

    private Task<ApplicationUser?> GetUserByIdWithProfileAsync(string id)
        => _users
            .All()
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);
    
    private async Task DeleteUserPictureIfPresent(ApplicationUser user)
    {
        if (!string.IsNullOrEmpty(user.Profile.ProfilePictureUrl))
        {
            await _cloudinaryService.DeleteFileAsync(user.Profile.ProfilePicturePublicId);
        }
    }
}