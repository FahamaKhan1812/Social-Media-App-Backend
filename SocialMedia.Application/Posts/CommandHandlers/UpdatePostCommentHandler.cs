using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.Posts.Commands;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.PostAggregate;

namespace SocialMedia.Application.Posts.CommandHandlers;
internal class UpdatePostCommentHandler : IRequestHandler<UpdatePostCommentCommand, OperationResult<PostComment>>
{
    private readonly DataContext _context;

    public UpdatePostCommentHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<PostComment>> Handle(UpdatePostCommentCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostComment>();
        try
        {
            var post = await _context.Posts
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);
            if (post == null)
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
           
            var comment = post.Comments.FirstOrDefault(c => c.CommentId == request.CommentId);
            if (comment == null)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.NotFound,
                    Message = "No comment Found"
                };
                result.Errors.Add(error);
                return result;
            }
            
            if (comment.UserProfileId != request.UserProfileId)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.CommentRemovalNotAuthorized,
                    Message = "Cannot remomve comment from post as you are not its author"
                };
                result.Errors.Add(error);
                return result;
            }

            comment.UpdateCommentText(request.UpdatedText);
            _context.Posts.Update(post);
            await _context.SaveChangesAsync(cancellationToken);
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
