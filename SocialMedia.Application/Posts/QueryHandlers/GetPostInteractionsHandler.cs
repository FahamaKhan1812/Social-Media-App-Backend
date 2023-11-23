using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.Posts.Queries;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.PostAggregate;

namespace SocialMedia.Application.Posts.QueryHandlers;
public class GetPostInteractionsHandler : IRequestHandler<GetPostInteractions, OperationResult<List<PostInteraction>>>
{
    private readonly DataContext _context;

    public GetPostInteractionsHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<List<PostInteraction>>> Handle(GetPostInteractions request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<PostInteraction>>();
        try
        {
            var posts = await _context.Posts
                .Include(p => p.Interactions)
                .ThenInclude(i => i.UserProfile)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);

            if(posts is null)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.NotFound,
                    Message = "No Post is Found"
                };
                result.Errors.Add(error);
                return result;
            }
            result.Payload = posts.Interactions.ToList();
        }
        catch (Exception ex)
        {
            Error error = new()
            {
                Code = ErrorCode.ServerError,
                Message = ex.Message,
            };
            result.IsError = true;
        }
        return result;

    }
}
