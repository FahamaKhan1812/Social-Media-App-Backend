using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.UserProfile.Commands;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.UserProfileAggregate;
using SocialMedia.Domain.Exceptions;

namespace SocialMedia.Application.UserProfile.CommandHandlers;
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>>
{
    private readonly DataContext _context;

    public UpdateUserCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>();

        try
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(user => user.UserProfileId == request.UserProfileId, cancellationToken);
            if(userProfile == null) 
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.NotFound,
                    Message  =  "No User is Found"
                };
                result.Errors.Add(error);
                return result;
            }
            BasicInfo basicInfo = BasicInfo.CreateBasicInfo(
                                  request.FirstName,
                                  request.LastName,
                                  request.EmailAddress,
                                  request.Phone,
                                  request.DateOfBirth,
                                  request.CurrentCity);

            userProfile.UpdateBasicInfo(basicInfo);
            _context.UserProfiles.Update(userProfile);
            await _context.SaveChangesAsync(cancellationToken);
            result.Payload = userProfile;
            return result;
        }
        catch (UserProfileNotValidException e)
        {
            result.IsError = true;
            e.ValidationErrors.ForEach(err =>
            {
                var errors = new Error
                {
                    Code = ErrorCode.ValidationError,
                    Message = $"{e.Message}"
                };
                result.Errors.Add(errors);
            });
            return result;
        }
        catch (Exception e)
        {
            Error error = new()
            {
                Code = ErrorCode.ServerError,
                Message = e.Message,
            };
            result.IsError = true;
            result.Errors.Add(error);
            return result;
         }
    }
}
