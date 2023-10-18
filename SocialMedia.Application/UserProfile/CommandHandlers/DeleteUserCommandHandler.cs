using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.UserProfile.Commands;
using SocialMedia.Dal.Data;

namespace SocialMedia.Application.UserProfile.CommandHandlers;
internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>>
{
    private readonly DataContext _context;

    public DeleteUserCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>();

        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(user => user.UserProfileId == request.UserProfileId, cancellationToken);

        if (userProfile is null)
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

        _context.UserProfiles.Remove(userProfile);
        await _context.SaveChangesAsync(cancellationToken);
        result.Payload  = userProfile;
        return result;
    }
}
