using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Identity.Commands;
using SocialMedia.Application.Models;
using SocialMedia.Dal.Data;

namespace SocialMedia.Application.Identity.CommandHandlers;
internal class RemoveAccountCommandHandler : IRequestHandler<RemoveAccountCommand, OperationResult<bool>>
{
    private readonly DataContext _dataContext;

    public RemoveAccountCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<OperationResult<bool>> Handle(RemoveAccountCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<bool>();
        try
        {
            var identityUser = await _dataContext.Users.FirstOrDefaultAsync(iu => iu.Id == request.IdentityUserId.ToString(), cancellationToken);

            if(identityUser is null)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.IdentityUserNotFound,
                    Message = "No Identity User is Found"
                };
                result.Errors.Add(error);
                return result;
            }

            var userProfile = await _dataContext.UserProfiles.FirstOrDefaultAsync(up => up.IdentityId == request.IdentityUserId.ToString(), cancellationToken);
            
            if(userProfile is null)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.NotFound,
                    Message = "No User is Found"
                };
                result.Errors.Add(error);
                return result;
            }
            
            if(identityUser.Id != request.RequestorGuid.ToString())
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.UnAuthorizedAccountRemoval,
                    Message = "Can not remove account as you are not the owner"
                };
                result.Errors.Add(error);
                return result;
            }

            _dataContext.UserProfiles.Remove(userProfile);
            _dataContext.Users.Remove(identityUser);
            await _dataContext.SaveChangesAsync(cancellationToken);

            result.Payload = true;
        }
        catch (Exception ex)
        {
            Error error = new()
            {
                Code = ErrorCode.ServerError,
                Message = ex.Message,
            };
            result.IsError = true;
            result.Errors.Add(error);
        }

        return result;
    }
}
