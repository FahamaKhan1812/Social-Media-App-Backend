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

    //Application Error should be in the range of 300 - 399
    PostUpdateUserNotPossible = 300,
    PostDeleteNotPossible = 301,
    RemoveInteractionUserNotValid =302,
    UnAuthorizedAccountRemoval = 306,

    UnknownError = 999
}