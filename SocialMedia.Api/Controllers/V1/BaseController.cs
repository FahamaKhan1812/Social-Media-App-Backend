using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Contracts.Common;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;

namespace SocialMedia.Api.Controllers.V1;

public class BaseController : ControllerBase
{
    protected IActionResult HandleErrorResponse(List<Error> errors)
    {
        ErrorResponse apiError = new();
        if (errors.Any(e => e.Code == ErrorCode.NotFound))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.NotFound);

            apiError.StatusCode = 404;
            apiError.StatusPhrase = "Not Found";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return NotFound(apiError);
        }

        apiError.StatusCode = 500;
        apiError.StatusPhrase = "Server Error";
        apiError.Timestamp = DateTime.Now;
        apiError.Errors.Add("Unknown Error");

        return StatusCode(500, apiError);
        

    }
}
