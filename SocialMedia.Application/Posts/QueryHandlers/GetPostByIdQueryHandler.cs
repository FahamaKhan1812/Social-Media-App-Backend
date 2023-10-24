using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.Posts.Queries;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.PostAggregates;

namespace SocialMedia.Application.Posts.QueryHandlers;
public class GetPostByIdQueryHandler : IRequestHandler<GetPostById, OperationResult<Post>>
{
    private readonly DataContext _context;

    public GetPostByIdQueryHandler(DataContext context)
    {
        _context = context;
    }

    public  async Task<OperationResult<Post>> Handle(GetPostById request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();
        try
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);
            if (post == null) 
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
            result.Payload = post;
        }
        catch (Exception e)
        {
            Error error = new()
            {
                Code = ErrorCode.ServerError,
                Message = e.Message,
            };
            result.IsError = true;
        }
        return result;
    }
}
