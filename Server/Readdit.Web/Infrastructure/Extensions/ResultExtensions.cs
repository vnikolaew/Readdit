using Microsoft.AspNetCore.Mvc;
using Readdit.Services.Data.Authentication.Models;

namespace Readdit.Web.Infrastructure.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this AuthenticationResultModel authResultModel)
        => authResultModel.Succeeded
            ? new OkObjectResult(new AuthenticationResultSuccessModel
            {
                Token = authResultModel.Token!,
                UserId = authResultModel.UserId!
            })
            : new BadRequestObjectResult(new AuthenticationResultErrorModel
            {
                Errors = authResultModel.Errors
            });
    
    public static IActionResult OkOrBadRequest<T>(this T result)
        => result is null
            ? new BadRequestResult()
            : new OkObjectResult(result);
    
    public static IActionResult OkOrNotFound<T>(this T result)
        => result is null
            ? new NotFoundResult()
            : new OkObjectResult(result);
    
    public static IActionResult OkOrBadRequest(this bool success)
        => success 
            ? new OkResult()
            : new BadRequestResult();
    
    public static IActionResult OkOrNotFound(this bool success)
        => success 
            ? new OkResult()
            : new NotFoundResult();
}