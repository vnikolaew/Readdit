using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Readdit.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
}