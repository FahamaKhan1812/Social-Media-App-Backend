using MediatR;
using SocialMedia.Application.Models;
using SocialMedia.Domain.Aggregates.PostAggregate;

namespace SocialMedia.Application.Posts.Commands;
public class AddInteraction : IRequest<OperationResult<PostInteraction>>
{
    public Guid PostId { get; set; }
    public Guid UserProfileId { get; set; }
    public InteractionType Type { get; set; }
}
