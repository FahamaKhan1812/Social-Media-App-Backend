using MediatR;
using SocialMedia.Application.Models;
using SocialMedia.Domain.Aggregates.PostAggregate;

namespace SocialMedia.Application.Posts.Commands;
public class RemoveCommentFromPost : IRequest<OperationResult<PostComment>>
{
    public Guid UserProfileId { get; set; }
    public Guid PostId { get; set; }
    public Guid CommentId { get; set; }
}
