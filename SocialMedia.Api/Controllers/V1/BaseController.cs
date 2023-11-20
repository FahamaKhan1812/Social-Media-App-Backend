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

        if (errors.Any(e => e.Code == ErrorCode.IdentityUserAlreadyExists))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.IdentityUserAlreadyExists);

            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad Request";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return BadRequest(apiError);
        }

        if (errors.Any(e => e.Code == ErrorCode.IdentityCreationFailed))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.IdentityCreationFailed);

            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad Request";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return BadRequest(apiError);
        }

        if (errors.Any(e => e.Code == ErrorCode.IncorrectPassword))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.IncorrectPassword);

            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad Request";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return BadRequest(apiError);
        }

        if (errors.Any(e => e.Code == ErrorCode.IdentityUserNotFound))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.IdentityUserNotFound);

            apiError.StatusCode = 404;
            apiError.StatusPhrase = "Not Found";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return NotFound(apiError);
        }

        if (errors.Any(e => e.Code == ErrorCode.PostUpdateUserNotPossible))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.PostUpdateUserNotPossible);

            apiError.StatusCode = 403;
            apiError.StatusPhrase = "Unauthenticated";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return new ObjectResult(apiError)
            {
                StatusCode = 403
            };
        }

        if (errors.Any(e => e.Code == ErrorCode.PostDeleteNotPossible))
        {
            var error = errors.FirstOrDefault(e => e.Code == ErrorCode.PostDeleteNotPossible);

            apiError.StatusCode = 403;
            apiError.StatusPhrase = "Unauthenticated";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add(error.Message);

            return new ObjectResult(apiError)
            {
                StatusCode = 403
            };
        }

        apiError.StatusCode = 500;
        apiError.StatusPhrase = "Server Error";
        apiError.Timestamp = DateTime.Now;
        apiError.Errors.Add("Unknown Error");

        return StatusCode(500, apiError);
        

    }
}
