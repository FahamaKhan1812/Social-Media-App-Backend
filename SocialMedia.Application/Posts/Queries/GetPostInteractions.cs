using MediatR;
using SocialMedia.Application.Models;
using SocialMedia.Domain.Aggregates.PostAggregate;

namespace SocialMedia.Application.Posts.Queries;
public class GetPostInteractions : IRequest<OperationResult<List<PostInteraction>>>
{
    public Guid PostId { get; set; }
}
