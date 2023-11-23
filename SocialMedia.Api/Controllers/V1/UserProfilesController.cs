using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Contracts.UserProfile.Responses;
using SocialMedia.Api.Filters;
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
}