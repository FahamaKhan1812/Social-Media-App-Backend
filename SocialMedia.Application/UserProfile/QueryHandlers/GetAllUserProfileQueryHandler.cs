using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Models;
using SocialMedia.Application.UserProfile.Queries;
using SocialMedia.Dal.Data;

namespace SocialMedia.Application.UserProfile.QueryHandlers;
internal class GetAllUserProfileQueryHandler : IRequestHandler<GetAllUserProfiles, OperationResult<IEnumerable<Domain.Aggregates.UserProfileAggregate.UserProfile>>>
{
    private readonly DataContext _context;

    public GetAllUserProfileQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<IEnumerable<Domain.Aggregates.UserProfileAggregate.UserProfile>>> Handle(GetAllUserProfiles request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<IEnumerable<Domain.Aggregates.UserProfileAggregate.UserProfile>>();

        result.Payload = await _context.UserProfiles.ToListAsync(cancellationToken);
        return result;
    }
}
