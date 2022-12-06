using Readdit.Services.Data.Authentication.Models;

namespace Readdit.Services.Data.Authentication;

public interface IAuthenticationService
{
    Task<string> PasswordLoginAsync(LoginInputModel loginInputModel);
    Task<string> RegisterAsync(RegisterInputModel registerInputModel);
}