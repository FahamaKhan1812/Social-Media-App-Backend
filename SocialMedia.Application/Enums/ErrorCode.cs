namespace SocialMedia.Application.Enums;
public enum ErrorCode
{
    NotFound = 404,
    ServerError = 500,
    ValidationError = 101,
    IdentityUserAlreadyExists = 201,
    IdentityCreationFailed = 202,
    IdentityUserNotFound = 203,
    IncorrectPassword = 204,
    UnknownError = 999
}