using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.UserProfile.Commands;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.UserProfileAggregate;

namespace SocialMedia.Application.UserProfile.CommandHandlers;
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly DataContext _context;

    public UpdateUserCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(user => user.UserProfileId == request.UserProfileId, cancellationToken);
        BasicInfo basicInfo = BasicInfo.CreateBasicInfo(
                              request.FirstName,
                              request.LastName,
                              request.EmailAddress,
                              request.Phone,
                              request.DateOfBirth,
                              request.CurrentCity);
        userProfile?.UpdateBasicInfo(basicInfo);
        _context.UserProfiles.Update(userProfile);
        await _context.SaveChangesAsync(cancellationToken);
        return new Unit();
    }
}
