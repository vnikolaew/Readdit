﻿using Microsoft.AspNetCore.Mvc;
using Readdit.Services.Data.Authentication.Models;

namespace Readdit.Web.Infrastructure.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this AuthenticationResultModel authResultModel)
        => authResultModel.Succeeded
            ? new OkObjectResult(new { authResultModel.Token, authResultModel.UserId })
            : new BadRequestObjectResult(new { authResultModel.Errors });
    
    public static IActionResult OkOrNotFound<T>(this T result)
        => result is null
            ? new NotFoundResult()
            : new OkObjectResult(result);
}