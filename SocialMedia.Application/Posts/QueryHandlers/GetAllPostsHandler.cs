using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.Posts.Queries;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.PostAggregates;

namespace SocialMedia.Application.Posts.QueryHandlers;

public class GetAllPostsHandler : IRequestHandler<GetAllPosts, OperationResult<List<Post>>>
{
    private readonly DataContext _context;

    public GetAllPostsHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<List<Post>>> Handle(GetAllPosts request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<Post>>();
        try
        {
            result.Payload = await _context.Posts.ToListAsync(cancellationToken);
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
