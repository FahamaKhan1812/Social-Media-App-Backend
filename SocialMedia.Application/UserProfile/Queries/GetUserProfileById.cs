using MediatR;
using SocialMedia.Application.Models;

namespace SocialMedia.Application.UserProfile.Queries;
public class GetUserProfileById : IRequest<OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>>
{
    public Guid UserProfileId { get; set; }
}
