using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Identity.Queries;
using SocialMedia.Application.Models;
using SocialMedia.Dal.Data;

namespace SocialMedia.Application.Identity.QueriesHandler;
internal class GetCurrentUserHandler : IRequestHandler<GetCurrentUser, OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>>
{
    private readonly DataContext _dataContext;
    private readonly UserManager<IdentityUser> _userManager;

    public GetCurrentUserHandler(DataContext dataContext, UserManager<IdentityUser> userManager)
    {
        _dataContext = dataContext;
        _userManager = userManager;
    }

    public async Task<OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>> Handle(GetCurrentUser request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>();
        try
        {
            var identity = await _userManager.FindByEmailAsync(request.UserEmail);
            if(identity == null)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.NotFound,
                    Message = "No user found"
                };
                result.Errors.Add(error);
                return result;
            }

            var profile = await _dataContext.UserProfiles.FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId, cancellationToken);
            result.Payload = profile!;
        }
        catch (Exception ex)
        {
            result.IsError = true;
            Error errors = new()
            {
                Code = ErrorCode.UnknownError,
                Message = ex.Message
            };
            result.Errors.Add(errors);
        }

        return result;
    }
}
