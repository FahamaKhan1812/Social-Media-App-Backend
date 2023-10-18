using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialMedia.Api.Contracts.Common;

namespace SocialMedia.Api.Filters;
public class ValidGuidAttribute : ActionFilterAttribute
{
    private readonly string _key;

    public ValidGuidAttribute(string key)
    {
        _key = key;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ActionArguments.TryGetValue(_key, out var value)) return;
        if (Guid.TryParse(value.ToString(), out var guid)) return;
        ErrorResponse apiError = new()
        {
            StatusCode = 400,
            StatusPhrase = "Bad Request",
            Timestamp = DateTime.Now
        };
        apiError.Errors.Add($"The identifier for {_key} is correct GUID Format");
        context.Result =  new ObjectResult(apiError );
    }
}
