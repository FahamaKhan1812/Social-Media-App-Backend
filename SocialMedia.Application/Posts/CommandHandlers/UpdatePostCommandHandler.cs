using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.Posts.Commands;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.PostAggregate;
using SocialMedia.Domain.Aggregates.PostAggregates;
using SocialMedia.Domain.Exceptions;

namespace SocialMedia.Application.Posts.CommandHandlers;
public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, OperationResult<Post>>
{
    private readonly DataContext _context;

    public UpdatePostCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<Post>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();
        try
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == request.PostId, cancellationToken);
            if(post is null)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.NotFound,
                    Message = "No User is Found"
                };
                result.Errors.Add(error);
                return result;
            }
            post.UpdatePost(request.TextContent);
            await _context.SaveChangesAsync(cancellationToken);
            result.Payload = post;
        }
        catch (PostNotValidException e)
        {
            result.IsError = true;
            e.ValidationErrors.ForEach(err =>
            {
                Error errors = new()
                {
                    Code = ErrorCode.ValidationError,
                    Message = $"{e.Message}"
                };
                result.Errors.Add(errors);
            });
        }
        catch (Exception e)
        {
            result.IsError = true;
            Error error = new()
            {
                Code = ErrorCode.ServerError,
                Message = e.Message,
            };
            result.Errors.Add(error);
        }
        return result;
    }
}
