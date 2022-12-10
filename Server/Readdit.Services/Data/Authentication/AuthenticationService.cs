using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Readdit.Common;
using Readdit.Infrastructure.Models;
using Readdit.Infrastructure.Models.Enums;
using Readdit.Services.Data.Authentication.Models;
using Readdit.Services.Data.Countries;
using Readdit.Services.External.Cloudinary;
using Readdit.Services.External.Cloudinary.Models;
using Readdit.Services.External.Messaging;

namespace Readdit.Services.Data.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICountryService _countryService;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IEmailSender _emailSender;
    private readonly IJwtService _jwtService;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        IJwtService jwtService,
        ICloudinaryService cloudinaryService,
        IEmailSender emailSender,
        ICountryService countryService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _countryService = countryService;
        _cloudinaryService = cloudinaryService;
        _emailSender = emailSender;
        _countryService = countryService;
    }

    public async Task<AuthenticationResultModel> PasswordLoginAsync(LoginInputModel loginInputModel)
    {
        var user = await _userManager.FindByNameAsync(loginInputModel.Username);
        if (user is null)
        {
            return AuthenticationResultModel
                .Failure("User with the specified username was not found");
        }

        var passwordMatch = await _userManager.CheckPasswordAsync(user, loginInputModel.Password);
        if (!passwordMatch)
        {
            return AuthenticationResultModel
                .Failure("Invalid credentials.");
        }

        (user.Profile ??= new()).Country = (await _countryService.GetByUserAsync(user.Id))!;
        var token = _jwtService.GenerateTokenForUser(user);
        return AuthenticationResultModel.Success(user.Id, token);
    }

    public async Task<AuthenticationResultModel> RegisterAsync(RegisterInputModel registerModel)
    {
        var country = await _countryService.GetByNameAsync(registerModel.Country);

        ImageUploadResult? uploadResult = null;
        if (registerModel.ProfilePicture != null)
        {
            var file = registerModel.ProfilePicture;
            
            uploadResult = await _cloudinaryService.UploadAsync(
                file.OpenReadStream(),
                file.FileName,
                file.ContentType);
        }

        var user = new ApplicationUser
        {
            UserName = registerModel.Username,
            FirstName = registerModel.FirstName,
            LastName = registerModel.LastName,
            Email = registerModel.Email,
            Profile = new UserProfile
            {
                ProfilePictureUrl = uploadResult?.AbsoluteImageUrl ?? string.Empty,
                ProfilePicturePublicId = uploadResult?.ImagePublidId ?? string.Empty,
                Country = country!,
                Gender = Enum.Parse<Gender>(registerModel.Gender),
            },
            Score = new UserScore()
        };

        var result = await _userManager.CreateAsync(user, registerModel.Password);
        if (!result.Succeeded)
        {
            return AuthenticationResultModel
                .Failure(result.Errors.Select(e => e.Description));
        }
        
        var emailConfirmationToken = await _userManager
            .GenerateEmailConfirmationTokenAsync(user);

        await _emailSender.SendEmailAsync(
            GlobalConstants.ReadditEmail,
            GlobalConstants.ApplicationName,
            user.Email,
            GlobalMessages.ConfirmProfileTitleMessage,
            string.Format(
                GlobalMessages.ConfirmProfileMessage,
                user.UserName,
                GetFullEmailConfirmationUrl(
                    registerModel.EmailConfirmationUrl,
                    emailConfirmationToken,
                    user.Id)));
            
        var token = _jwtService.GenerateTokenForUser(user);
        return AuthenticationResultModel.Success(user.Id, token);
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string emailConfirmToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return false;
        }

        var result = await _userManager.ConfirmEmailAsync(user, emailConfirmToken);
        return result.Succeeded;
    }

    private static string GetFullEmailConfirmationUrl(
        string emailConfirmEmail,
        string token,
        string userId)
        => QueryHelpers.AddQueryString(
            emailConfirmEmail,
            new Dictionary<string, string?>
            {
                { nameof(token), token },
                { nameof(userId), userId },
            });
}