using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Contracts.Common;
using SocialMedia.Api.Contracts.Posts.Requests;
using SocialMedia.Api.Contracts.Posts.Responses;
using SocialMedia.Api.Extensions;
using SocialMedia.Api.Filters;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.Posts.CommandHandlers;
using SocialMedia.Application.Posts.Commands;
using SocialMedia.Application.Posts.Queries;
using SocialMedia.Domain.Aggregates.PostAggregate;
using SocialMedia.Domain.Aggregates.PostAggregates;
using System.Security.Claims;

namespace SocialMedia.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.baseRoute)]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class PostController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public PostController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var query = new GetAllPosts();
        var response = await _mediator.Send(query);
        var result = _mapper.Map<List<PostResponse>>(response.Payload);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(result);
    }
   
    [HttpGet]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidGuid("id")]
    public async Task<IActionResult> GetById(string id)
    {
        var query = new GetPostById { PostId = Guid.Parse(id) };
        var response = await _mediator.Send(query);
        var post = _mapper.Map<PostResponse>(response.Payload);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(post);
    }

    [HttpPost]
    [ValideModel]
    public async Task<IActionResult> CreatePost([FromBody] PostCreate postCreate)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new CreatePostCommand()
        {
            UserId = userProfileId,
            TextContext = postCreate.TextContent
        };
        var result = await _mediator.Send(command);
        var mapped = _mapper.Map<PostResponse>(result.Payload);
        
        return result.IsError ? HandleErrorResponse(result.Errors) 
            : CreatedAtAction(nameof(GetById), new {id = result.Payload.PostId}, mapped);
    }


    [HttpPatch]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidGuid("id")]
    [ValideModel]
    public async Task<IActionResult> UpdatePost(string id, [FromBody] UpdatePost updatePost)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();

        var command = new UpdatePostCommand()
        {
            PostId = Guid.Parse(id),
            TextContent = updatePost.Text,
            UserProfileId = userProfileId
        };
        var resposne = await _mediator.Send(command);
        return resposne.IsError ? HandleErrorResponse(resposne.Errors) : NoContent();
    }

    [HttpDelete]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidGuid("id")]
    public async Task<IActionResult> DeletePost(string id)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new DeletePostCommand() 
        { 
            PostId = Guid.Parse(id),
            UserPorfileId = userProfileId
        };
        var resposne = await _mediator.Send(command);
        return resposne.IsError ? HandleErrorResponse(resposne.Errors) : NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.Posts.PostComment)]
    [ValidGuid("postId")]
    public async Task<IActionResult> GetPostCommentsById(string postId)
    {
        var query = new GetPostComments() { PostId = Guid.Parse(postId)};

        var result = await _mediator.Send(query);
        if (result.IsError) HandleErrorResponse(result.Errors);

        var comments = _mapper.Map<List<PostCommentResponse>>(result.Payload);
        return Ok(comments);
    }

    [HttpPost]
    [Route(ApiRoutes.Posts.PostComment)]
    [ValidGuid("postId")]
    [ValideModel]
    public async Task<IActionResult> AddCommentToPost(string postId, [FromBody] PostCommentCreate comment)
    {
        var userProfileId = HttpContext.GetUserProfileIdClaimValue();
        var command = new AddPostComment()
        {
            PostId = Guid.Parse(postId),
            UserProfileId = userProfileId,
            CommentsText = comment.Text
        };

        var result = await _mediator.Send(command);
        if (result.IsError) HandleErrorResponse(result.Errors);
        var newComment = _mapper.Map<PostCommentResponse>(result.Payload);
        return Ok(newComment);
    }
    
    [HttpDelete]
    [Route(ApiRoutes.Posts.CommentById)]
    [ValidGuid("postId", "commentId")]
    public async Task<IActionResult> RemoveCommentFromPost(string postId, string commentId)
    {
        Guid userProfileId = HttpContext.GetUserProfileIdClaimValue();
        Guid PostGuid = Guid.Parse(postId);
        Guid commentGuid = Guid.Parse(commentId);
        var command = new RemoveCommentFromPost()
        {
            UserProfileId = userProfileId,
            PostId = PostGuid,
            CommentId = commentGuid,
        };
        var result = await _mediator.Send(command);
        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpPut]
    [Route(ApiRoutes.Posts.CommentById)]
    [ValidGuid("postId", "commentId")]
    [ValideModel]
    public async Task<IActionResult> UpdatePostComment(string postId, string commentId, PostCommentUpdate updatedComment)
    {
        Guid userProfileId = HttpContext.GetUserProfileIdClaimValue();
        Guid PostGuid = Guid.Parse(postId);
        Guid commentGuid = Guid.Parse(commentId);
        var command = new UpdatePostCommentCommand()
        {
            UserProfileId = userProfileId,
            CommentId = commentGuid,
            PostId = PostGuid,
            UpdatedText = updatedComment.Text
        };
        var result = await _mediator.Send(command);
        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpGet]
    [Route(ApiRoutes.Posts.PostInteractions)]
    [ValidGuid("postId")]
    public async Task<IActionResult> GetPostInteraction(string postId) 
    {
        var postGuid = Guid.Parse(postId);
        var query = new GetPostInteractions()
        {
            PostId = postGuid
        };
        var result = await _mediator.Send(query);
        var mapped = _mapper.Map<List<PostInteractionResponse>>(result.Payload);
        return result.IsError ? HandleErrorResponse(result.Errors) : Ok(mapped);
    }

    [HttpPost]
    [Route(ApiRoutes.Posts.PostInteractions)]
    [ValidGuid("postId")]
    [ValideModel]
    public async Task<IActionResult> AddPostInteraction(string postId, [FromBody] PostInteractionCreate create)
    {
        var postGuid = Guid.Parse(postId);
        var userProfile = HttpContext.GetUserProfileIdClaimValue();
        var command = new AddInteraction()
        {
            PostId = postGuid,
            Type = create.Type,
            UserProfileId = userProfile
        };
        var result = await _mediator.Send(command);
        var mapped = _mapper.Map<PostInteractionResponse>(result.Payload);
        return result.IsError ? HandleErrorResponse(result.Errors) : Ok(mapped);

    }

    [HttpDelete]
    [Route(ApiRoutes.Posts.InteractionById)]
    [ValidGuid("postId", "interactionId")]
    public async Task<IActionResult> RemovePostInteraction(string postId, string interactionId)
    {
        Guid postGuid = Guid.Parse(postId);
        Guid interactionGuid = Guid.Parse(interactionId);
        Guid userProfileGuid = HttpContext.GetUserProfileIdClaimValue();
        var command = new RemovePostInteractionCommand()
        {
            InteractionId = interactionGuid,
            PostId = postGuid,
            UserProfileId = userProfileGuid
        };

        var result = await _mediator.Send(command);
        var mapped = _mapper.Map<PostInteractionResponse>(result.Payload);
        return result.IsError ? HandleErrorResponse(result.Errors) : Ok(mapped);
    }
}
