using MediatR;
using SocialMedia.Application.Models;
using SocialMedia.Domain.Aggregates.PostAggregates;

namespace SocialMedia.Application.Posts.Queries;
public class GetPostById : IRequest<OperationResult<Post>>
{
    public Guid PostId { get; set; }
}
