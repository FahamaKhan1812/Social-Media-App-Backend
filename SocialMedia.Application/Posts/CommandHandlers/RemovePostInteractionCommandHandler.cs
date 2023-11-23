using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.Posts.Commands;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.PostAggregate;

namespace SocialMedia.Application.Posts.CommandHandlers;
internal class RemovePostInteractionCommandHandler : IRequestHandler<RemovePostInteractionCommand, OperationResult<PostInteraction>>
{
    private readonly DataContext _context;

    public RemovePostInteractionCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<PostInteraction>> Handle(RemovePostInteractionCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<PostInteraction>();
        try
        {
            var post = await _context.Posts
                .Include(p => p.Interactions)
                .ThenInclude(i => i.UserProfile)
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

            var interaction = post.Interactions.FirstOrDefault(i => i.InteractionId == request.InteractionId);
            if (interaction is null)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.NotFound,
                    Message = "No interaction is Found"
                };
                result.Errors.Add(error);
                return result;
            }
            
            if(interaction.UserProfileId  != request.UserProfileId)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.RemoveInteractionUserNotValid,
                    Message = "Cannot remove the interaction because user is not valid"
                };
                result.Errors.Add(error);
                return result;
            }
           
            post.RemoveInteraction(interaction);
            _context.Posts.Update(post);
            await _context.SaveChangesAsync(cancellationToken);
            result.Payload = interaction;

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