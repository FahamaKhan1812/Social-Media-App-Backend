using MediatR;
using SocialMedia.Application.Models;
using SocialMedia.Domain.Aggregates.PostAggregate;

namespace SocialMedia.Application.Posts.Commands;
public class RemovePostInteractionCommand : IRequest<OperationResult<PostInteraction>>
{
    public Guid PostId { get; set; }
    public Guid InteractionId { get; set; }
    public Guid UserProfileId { get; set; }
}
