using Readdit.Services.Data.Authentication.Models;

namespace Readdit.Services.Data.Authentication;

public interface IAuthenticationService
{
    Task<AuthenticationResultModel> PasswordLoginAsync(LoginInputModel loginInputModel);
    Task<AuthenticationResultModel> RegisterAsync(RegisterInputModel registerModel);
    Task<bool> ConfirmEmailAsync(string userId, string emailConfirmToken);
}