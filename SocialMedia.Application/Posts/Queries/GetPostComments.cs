using MediatR;
using SocialMedia.Application.Models;
using SocialMedia.Domain.Aggregates.PostAggregate;

namespace SocialMedia.Application.Posts.Queries;
public class GetPostComments : IRequest<OperationResult<List<PostComment>>>
{
    public Guid PostId { get; set; }
}
