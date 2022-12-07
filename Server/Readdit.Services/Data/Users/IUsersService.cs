using Microsoft.AspNetCore.Http;
using Readdit.Infrastructure.Models;

namespace Readdit.Services.Data.Users;

public interface IUsersService
{
    Task<ApplicationUser?> GetUserByIdAsync(string id);
    
    Task<T?> GetUserInfoAsync<T>(string id);

    Task<ApplicationUser?> UpdateUserProfileAsync(
        string userId,
        string firstName,
        string lastName,
        string gender,
        string country,
        string aboutContent, 
        IFormFile? profilePicture
    );
}