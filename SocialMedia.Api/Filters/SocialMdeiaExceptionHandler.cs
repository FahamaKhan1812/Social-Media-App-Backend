using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialMedia.Api.Contracts.Common;

namespace SocialMedia.Api.Filters;
public class SocialMdeiaExceptionHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        ErrorResponse apiError = new();
        apiError.StatusCode = 500;
        apiError.StatusPhrase = "Internal Server Error";
        apiError.Timestamp = DateTime.Now;
        apiError.Errors.Add(context.Exception.Message);


        context.Result = new JsonResult(apiError)
        {
            StatusCode = 500,
        };
    }
}
