using MediatR;

namespace SocialMedia.Application.UserProfile.Commands;
public class DeleteUserCommand : IRequest
{
    public Guid UserProfileId { get; set;}
}
