using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.Posts.Commands;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.PostAggregate;

namespace SocialMedia.Application.Posts.CommandHandlers;
internal class AddInteractionCommandHandler : IRequestHandler<AddInteraction, OperationResult<PostInteraction>>
{
    private readonly DataContext _dataContext;

    public AddInteractionCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<OperationResult<PostInteraction>> Handle(AddInteraction request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostInteraction>();

        try
        {
            var post = await _dataContext.Posts
                .Include(p => p.Interactions)
                .FirstOrDefaultAsync(p => p.PostId == request.PostId, cancellationToken);
            
            if(post is null)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.NotFound,
                    Message = "No post is Found"
                };
                result.Errors.Add(error);
                return result;
            }
            
            var interaction = PostInteraction.CreatePostInteraction(request.PostId, request.Type, request.UserProfileId);
            post.AddInteraction(interaction);

            _dataContext.Posts.Update(post);
            await _dataContext.SaveChangesAsync(cancellationToken);
            result.Payload = interaction;
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
