using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Common.Repositories;
using Readdit.Infrastructure.Data.Repositories;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.Authentication.Models;
using Readdit.Services.External.Cloudinary;

namespace Readdit.Services.Data.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<Country> _countryRepo;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IJwtService _jwtService;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        IRepository<Country> countryRepo,
        ICloudinaryService cloudinaryService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _countryRepo = countryRepo;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<string> PasswordLoginAsync(LoginInputModel loginInputModel)
    {
        var user = await _userManager.FindByNameAsync(loginInputModel.Username);
        if (user is null)
        {
            return null;
        }

        var passwordMatch = await _userManager.CheckPasswordAsync(user, loginInputModel.Password);
        if (!passwordMatch)
        {
            return null;
        }

        return _jwtService.GenerateTokenForUser(user);
    }

    public async Task<string> RegisterAsync(RegisterInputModel registerInputModel)
    {
        var country = await _countryRepo
            .All()
            .FirstOrDefaultAsync(c => c.Name == registerInputModel.Country);

        string? profilePictureUrl = null;
        if (registerInputModel.ProfilePicture != null)
        {
            var file = registerInputModel.ProfilePicture;
            
            profilePictureUrl = await _cloudinaryService.UploadAsync(
                file.OpenReadStream(),
                file.FileName,
                file.ContentType);
        }

        var user = new ApplicationUser
        {
            UserName = registerInputModel.Username,
            FirstName = registerInputModel.FirstName,
            LastName = registerInputModel.LastName,
            Country = country!,
            Email = registerInputModel.Email,
            Gender = Enum.Parse<Gender>(registerInputModel.Gender),
            Profile = new UserProfile
            {
                ProfilePictureUrl = profilePictureUrl!,
            },
        };

        var result = await _userManager.CreateAsync(user, registerInputModel.Password);
        if (result.Succeeded)
        {
            return _jwtService.GenerateTokenForUser(user);
        }

        return string.Empty;
    }
}