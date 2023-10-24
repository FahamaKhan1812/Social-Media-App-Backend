using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.Posts.Queries;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.PostAggregate;
using SocialMedia.Domain.Aggregates.PostAggregates;

namespace SocialMedia.Application.Posts.QueryHandlers;
public class GetPostCommentHandler : IRequestHandler<GetPostComments, OperationResult<List<PostComment>>>
{
    private readonly DataContext _context;

    public GetPostCommentHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<List<PostComment>>> Handle(GetPostComments request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<PostComment>>();

        try
        {
            var posts = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId);

            result.Payload = posts.Comments.ToList();  

        }
        catch (Exception e)
        {
            result.IsError = true;
            Error errors = new()
            {
                Code = ErrorCode.UnknownError,
                Message = e.Message,
            };
            result.Errors.Add(errors);
        }    
        return result;
    }
}
