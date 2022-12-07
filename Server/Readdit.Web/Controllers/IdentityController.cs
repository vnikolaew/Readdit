using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Readdit.Services.Data.Authentication;
using Readdit.Services.Data.Authentication.Models;
using Readdit.Web.Infrastructure.Extensions;

namespace Readdit.Web.Controllers;

public class IdentityController : ApiController
{
    private readonly IAuthenticationService _authenticationService;

    public IdentityController(IAuthenticationService authenticationService)
        => _authenticationService = authenticationService;

    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromForm] RegisterInputModel registerInputModel)
    {
        var result = await _authenticationService.RegisterAsync(registerInputModel);
        return result.ToActionResult();
    }
    
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromForm] LoginInputModel loginInputModel)
    {
        var result = await _authenticationService.PasswordLoginAsync(loginInputModel);
        return result.ToActionResult();
    }
}