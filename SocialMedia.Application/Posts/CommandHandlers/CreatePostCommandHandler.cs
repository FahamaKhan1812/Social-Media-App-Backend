using MediatR;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.Posts.Commands;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.PostAggregates;
using SocialMedia.Domain.Exceptions;

namespace SocialMedia.Application.Posts.CommandHandlers;
public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, OperationResult<Post>>
{
    private readonly DataContext _dataContext;

    public CreatePostCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<OperationResult<Post>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();
        try
        {
            Post post = Post.CreatePost(request.UserId, request.TextContext);
            await _dataContext.Posts.AddAsync(post, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);
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
        catch (Exception ex)
        {
            result.IsError = true;
            Error errors = new()
            {
                Code = ErrorCode.UnknownError, 
                Message = ex.Message 
            };
            result.Errors.Add(errors);
        }
        return result;
    }
}
