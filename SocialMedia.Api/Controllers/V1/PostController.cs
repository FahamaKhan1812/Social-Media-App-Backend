using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Contracts.Common;
using SocialMedia.Api.Contracts.Posts.Requests;
using SocialMedia.Api.Contracts.Posts.Responses;
using SocialMedia.Api.Filters;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Models;
using SocialMedia.Application.Posts.CommandHandlers;
using SocialMedia.Application.Posts.Commands;
using SocialMedia.Application.Posts.Queries;
using SocialMedia.Domain.Aggregates.PostAggregates;

namespace SocialMedia.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.baseRoute)]
[ApiController]
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
        var command = new CreatePostCommand()
        {
            UserId = postCreate.UserProfileId,
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
        var command = new UpdatePostCommand()
        {
            PostId = Guid.Parse(id),
            TextContent = updatePost.Text
        };
        var resposne = await _mediator.Send(command);
        return resposne.IsError ? HandleErrorResponse(resposne.Errors) : NoContent();
    }

    [HttpDelete]
    [Route(ApiRoutes.Posts.IdRoute)]
    [ValidGuid("id")]
    public async Task<IActionResult> DeletePost(string id)
    {
        var command = new DeletePostCommand() { PostId = Guid.Parse(id) };
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
        var isValidGuid = Guid.TryParse(comment.UserProfileId, out Guid userProfileId);
        if (!isValidGuid) 
        {
            ErrorResponse apiError = new();
            apiError.StatusCode = 400;
            apiError.StatusPhrase = "Bad Request";
            apiError.Timestamp = DateTime.Now;
            apiError.Errors.Add("Not Valid Guid");
            return BadRequest(apiError);
        }
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
}
