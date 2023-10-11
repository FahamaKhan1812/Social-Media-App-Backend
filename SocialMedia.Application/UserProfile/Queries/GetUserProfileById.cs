using MediatR;

namespace SocialMedia.Application.UserProfile.Queries;
public class GetUserProfileById : IRequest<Domain.Aggregates.UserProfileAggregate.UserProfile>
{
    public Guid UserProfileId { get; set; }
}
