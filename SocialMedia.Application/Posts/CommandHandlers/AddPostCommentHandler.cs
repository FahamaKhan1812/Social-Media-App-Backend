using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.Posts.Commands;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.PostAggregate;
using SocialMedia.Domain.Exceptions;

namespace SocialMedia.Application.Posts.CommandHandlers;
public class AddPostCommentHandler : IRequestHandler<AddPostComment, OperationResult<PostComment>>
{
    private readonly DataContext _dataContext;

    public AddPostCommentHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<OperationResult<PostComment>> Handle(AddPostComment request, CancellationToken cancellationToken)
    {
      var result = new OperationResult<PostComment>();
        try
        {
            var post = await _dataContext.Posts.FirstOrDefaultAsync(x => x.PostId == request.PostId, cancellationToken);
            if (post is null)
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
            var comment = PostComment.CreatePostComment(request.PostId, request.CommentsText, request.UserProfileId);
            post.AddPostComment(comment);
            _dataContext.Posts.Update(post);    
            await _dataContext.SaveChangesAsync(cancellationToken);
            result.Payload = comment;
        }
        catch (PostCommentNotValidException e)
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
            Error errors = new()
            {
                Code = ErrorCode.UnknownError,
                Message = e.Message
            };
            result.Errors.Add(errors);
        }
        return result;
    }
}
