using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using SocialMedia.Api.Contracts.Common;

namespace SocialMedia.Api.Filters;
public class ValideModelAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if(!context.ModelState.IsValid)
        {
            ErrorResponse apiError = new()
            {
                StatusCode = 400,
                StatusPhrase = "Bad Request",
                Timestamp = DateTime.Now
            };
            var errors = context.ModelState.AsEnumerable();
            foreach (var error in errors)
            {
                foreach (var inner in error.Value.Errors)
                {
                    apiError.Errors.Add(inner.ErrorMessage);
                }
            }

            context.Result = new BadRequestObjectResult(apiError);
        }
    }
}