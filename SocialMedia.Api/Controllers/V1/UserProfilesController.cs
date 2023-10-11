using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Contracts.UserProfile.Requests;
using SocialMedia.Api.Contracts.UserProfile.Responses;
using SocialMedia.Application.UserProfile.Commands;
using SocialMedia.Application.UserProfile.Queries;

namespace SocialMedia.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.baseRoute)]
[ApiController]
public class UserProfilesController : ControllerBase
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
        var profiles = _mapper.Map<List<UserProfileResponse>>(response);
        return Ok(profiles);
    }

    [HttpGet(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> GetUserProfileById(string id)
    {
        var query = new GetUserProfileById { UserProfileId = Guid.Parse(id) };
        var response = await _mediator.Send(query);
        var userProfile = _mapper.Map<UserProfileResponse>(response);
        if(userProfile is null)
        {
            return NotFound();
        }
        return Ok(userProfile);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserProfile([FromBody] UserProfileCreateUpdate profile)
    {
        var command = _mapper.Map<CreateUserCommand>(profile);
        var response = await _mediator.Send(command);
        var userProfile = _mapper.Map<UserProfileResponse>(response);

        return CreatedAtAction(nameof(GetUserProfileById), new { id = response.UserProfileId}, userProfile);
    }

    [HttpPatch(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> UpdateUserProfile(string id, UserProfileCreateUpdate updatedProfile)
    {
        var command = _mapper.Map<UpdateUserCommand>(updatedProfile);
        command.UserProfileId = Guid.Parse(id);
        var response = await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> DeleteUserProfile(string id) 
    {
        var command = new DeleteUserCommand() { UserProfileId = Guid.Parse(id) };
        var response = await _mediator.Send(command);
        return NoContent();
    }
}