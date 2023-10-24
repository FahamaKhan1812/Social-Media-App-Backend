using MediatR;
using SocialMedia.Application.Models;
using SocialMedia.Domain.Aggregates.PostAggregates;

namespace SocialMedia.Application.Posts.Commands;
public class CreatePostCommand : IRequest<OperationResult<Post>>
{
    public Guid UserId { get; set; }
    public string TextContext { get; set; }
}
