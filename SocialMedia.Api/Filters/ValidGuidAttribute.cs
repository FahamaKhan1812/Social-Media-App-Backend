using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SocialMedia.Api.Contracts.Common;

namespace SocialMedia.Api.Filters;
public class ValidGuidAttribute : ActionFilterAttribute
{
    private readonly List<string> _keys;

    public ValidGuidAttribute(string key)
    {
        _keys = new List<string>
        {
            key
        };
    }

    public ValidGuidAttribute(string key1, string key2)
    {
        _keys = new List<string>
        {
            key1,
            key2
        };
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        bool hasError = false;
        ErrorResponse apiError = new();
        _keys.ForEach(k =>
        {
            if (!context.ActionArguments.TryGetValue(k, out var value)) return;
            if (!Guid.TryParse(value.ToString(), out var guid))
            {
                hasError = true;
                apiError.Errors.Add($"The identifier for {k} is not a correct GUID Format");
            }
        });
        if (hasError)
        {
            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad Request";
            apiError.Timestamp = DateTime.Now;
            context.Result = new ObjectResult(apiError);
        }
    }
}
