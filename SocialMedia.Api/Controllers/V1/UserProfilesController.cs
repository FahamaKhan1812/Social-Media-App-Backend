using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Contracts.Common;
using SocialMedia.Api.Contracts.UserProfile.Requests;
using SocialMedia.Api.Contracts.UserProfile.Responses;
using SocialMedia.Api.Filters;
using SocialMedia.Application.Enums;
using SocialMedia.Application.UserProfile.Commands;
using SocialMedia.Application.UserProfile.Queries;

namespace SocialMedia.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.baseRoute)]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserProfilesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public UserProfilesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllProfiles()
    {
        var query = new GetAllUserProfiles();
        var response = await _mediator.Send(query);
        var profiles = _mapper.Map<List<UserProfileResponse>>(response.Payload);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(profiles);
    }

    [HttpGet(ApiRoutes.UserProfiles.IdRoute)]
    [ValidGuid("id")]
    public async Task<IActionResult> GetUserProfileById(string id)
    {
        var query = new GetUserProfileById { UserProfileId = Guid.Parse(id) };
        var response = await _mediator.Send(query);
        var userProfile = _mapper.Map<UserProfileResponse>(response.Payload);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(userProfile);
    }

    [HttpPatch(ApiRoutes.UserProfiles.IdRoute)]
    [ValideModel]
    [ValidGuid("id")]
    public async Task<IActionResult> UpdateUserProfile(string id, UserProfileCreateUpdate updatedProfile)
    {
        var command = _mapper.Map<UpdateUserCommand>(updatedProfile);
        command.UserProfileId = Guid.Parse(id);
        var response = await _mediator.Send(command);
        return response.IsError ? HandleErrorResponse(response.Errors) : NoContent();
    }

    [HttpDelete(ApiRoutes.UserProfiles.IdRoute)]
    [ValidGuid("id")]
    public async Task<IActionResult> DeleteUserProfile(string id) 
    {
        var command = new DeleteUserCommand() { UserProfileId = Guid.Parse(id) };
        var response = await _mediator.Send(command);
        return response.IsError ? HandleErrorResponse(response.Errors) : NoContent();
    }
}