using Microsoft.AspNetCore.Mvc;
using Readdit.Services.Data.Users;
using Readdit.Services.Data.Users.Models;
using Readdit.Web.Infrastructure.Extensions;

namespace Readdit.Web.Controllers;

public class UsersController : ApiController
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
        => _usersService = usersService;

    [HttpGet]
    [Route("{userId}")]
    [ResponseCache(Duration = 60 * 10, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Details(string userId)
    {
        var userDetails = await _usersService.GetUserInfoAsync<UserDetailsServiceModel>(userId);
        return userDetails.OkOrNotFound();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateProfile(
        [FromForm] UpdateUserProfileModel model) 
    {
        var user = await _usersService.UpdateUserProfileAsync(
            User.GetId()!,
            model.FirstName,
            model.LastName,
            model.Gender,
            model.Country,
            model.AboutContent,
            model.Picture);

        return user is null
            ? BadRequest()
            : AcceptedAtAction(nameof(Details), new { userId = user.Id }, user);
    }
}