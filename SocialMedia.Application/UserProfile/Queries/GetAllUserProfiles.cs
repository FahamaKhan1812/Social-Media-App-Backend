using MediatR;
using SocialMedia.Application.Models;

namespace SocialMedia.Application.UserProfile.Queries;
public class GetAllUserProfiles : IRequest<OperationResult<IEnumerable<Domain.Aggregates.UserProfileAggregate.UserProfile>>>
{

}
