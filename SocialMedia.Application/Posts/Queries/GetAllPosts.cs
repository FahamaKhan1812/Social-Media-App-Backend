using MediatR;
using SocialMedia.Application.Models;
using SocialMedia.Domain.Aggregates.PostAggregates;

namespace SocialMedia.Application.Posts.Queries;
public class GetAllPosts : IRequest<OperationResult<List<Post>>>
{
}
