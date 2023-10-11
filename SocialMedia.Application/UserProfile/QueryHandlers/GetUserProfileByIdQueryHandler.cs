
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.UserProfile.Queries;
using SocialMedia.Dal.Data;

namespace SocialMedia.Application.UserProfile.QueryHandlers;
internal class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileById, Domain.Aggregates.UserProfileAggregate.UserProfile>
{
    private readonly DataContext _context;

    public GetUserProfileByIdQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Domain.Aggregates.UserProfileAggregate.UserProfile> Handle(GetUserProfileById request, CancellationToken cancellationToken)
    {
        return await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId,cancellationToken);
    }
}
