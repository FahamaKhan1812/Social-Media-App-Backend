 using MediatR;
using SocialMedia.Application.Models;
using SocialMedia.Domain.Aggregates.PostAggregates;

namespace SocialMedia.Application.Posts.Commands;
public class UpdatePostCommand : IRequest<OperationResult<Post>>
{
    public Guid PostId { get; set; }
    public string TextContent { get; set; }
}
