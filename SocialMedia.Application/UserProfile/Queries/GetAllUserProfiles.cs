using MediatR;

namespace SocialMedia.Application.UserProfile.Queries;
public class GetAllUserProfiles : IRequest<IEnumerable<Domain.Aggregates.UserProfileAggregate.UserProfile>>
{

}
