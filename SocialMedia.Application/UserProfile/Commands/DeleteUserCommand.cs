using MediatR;
using SocialMedia.Application.Models;

namespace SocialMedia.Application.UserProfile.Commands;
public class DeleteUserCommand : IRequest<OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>>
{
    public Guid UserProfileId { get; set;}
}
