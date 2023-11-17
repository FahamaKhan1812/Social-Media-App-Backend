using MediatR;
using SocialMedia.Application.Models;
using SocialMedia.Domain.Aggregates.PostAggregates;

namespace SocialMedia.Application.Posts.Commands;
public class DeletePostCommand : IRequest<OperationResult<Post>>
{
    public Guid PostId { get; set; }
    public Guid UserPorfileId { get; set; }
}
