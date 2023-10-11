using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.UserProfile.Commands;
using SocialMedia.Dal.Data;

namespace SocialMedia.Application.UserProfile.CommandHandlers;
internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly DataContext _context;

    public DeleteUserCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(user => user.UserProfileId == request.UserProfileId, cancellationToken);
        
        // TODO: Return type will be decided later
        //if(userProfile is null)
        //{
        //    return new Unit();
        //}
        
        _context.UserProfiles.Remove(userProfile);
        await _context.SaveChangesAsync(cancellationToken);
        return new Unit();
    }
}
