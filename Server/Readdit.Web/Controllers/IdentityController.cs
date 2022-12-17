using System.Net;
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
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(AuthenticationResultSuccessModel))]
    [ProducesResponseType((int) HttpStatusCode.BadRequest, Type = typeof(AuthenticationResultErrorModel))]
    public async Task<IActionResult> RegisterAsync([FromForm] RegisterInputModel registerInputModel)
    {
        var result = await _authenticationService.RegisterAsync(registerInputModel);
        return result.ToActionResult();
    }
    
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(AuthenticationResultSuccessModel))]
    [ProducesResponseType((int) HttpStatusCode.BadRequest, Type = typeof(AuthenticationResultErrorModel))]
    public async Task<IActionResult> LoginAsync(LoginInputModel loginInputModel)
    {
        var result = await _authenticationService.PasswordLoginAsync(loginInputModel);
        return result.ToActionResult();
    }

    [HttpPost]
    [Route("confirmEmail")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ConfirmEmailAsync(
        [FromQuery] string userId, [FromQuery] string token)
    {
        var success = await _authenticationService.ConfirmEmailAsync(userId, token);
        return success.OkOrBadRequest();
    }
}