using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Readdit.Web.Infrastructure.Extensions;

public static class ModelStateExtensions
{
    public static IDictionary<string, IEnumerable<string>> ToDictionary(
        this ModelStateDictionary modelState)
        => modelState
            .ToDictionary(e => e.Key,
                e => e.Value?
                    .Errors
                    .Select(e => e.ErrorMessage));
}