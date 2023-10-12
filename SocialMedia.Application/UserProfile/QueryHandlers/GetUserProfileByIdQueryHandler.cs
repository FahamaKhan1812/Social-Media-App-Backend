
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.UserProfile.Queries;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.UserProfileAggregate;

namespace SocialMedia.Application.UserProfile.QueryHandlers;
internal class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileById, OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>>
{
    private readonly DataContext _context;

    public GetUserProfileByIdQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>> Handle(GetUserProfileById request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>();
        try
        {
            var userProfile =  await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId,cancellationToken);
            if (userProfile == null)
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
            result.Payload = userProfile;
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
        }

        return result;
    }
}
