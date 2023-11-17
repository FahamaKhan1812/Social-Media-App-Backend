using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.Posts.Commands;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.PostAggregates;

namespace SocialMedia.Application.Posts.CommandHandlers;
public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, OperationResult<Post>>
{
    private readonly DataContext _context;

    public DeletePostCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<Post>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
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
                    Message = "No Post Found"
                };
                result.Errors.Add(error);
                return result;
            }
            if(post.UserProfileId != request.UserPorfileId)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.PostDeleteNotPossible,
                    Message = "Not a valid user to delete the post."
                };
                result.Errors.Add(error);
                return result;
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync(cancellationToken);
            result.Payload = post;
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
