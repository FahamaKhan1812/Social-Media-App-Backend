using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.UserProfile.Queries;
using SocialMedia.Dal.Data;

namespace SocialMedia.Application.UserProfile.QueryHandlers;
internal class GetAllUserProfileQueryHandler : IRequestHandler<GetAllUserProfiles, IEnumerable<Domain.Aggregates.UserProfileAggregate.UserProfile>>
{
    private readonly DataContext _context;

    public GetAllUserProfileQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Domain.Aggregates.UserProfileAggregate.UserProfile>> Handle(GetAllUserProfiles request, CancellationToken cancellationToken)
    {
        return await _context.UserProfiles.ToListAsync(cancellationToken);
    }
}
