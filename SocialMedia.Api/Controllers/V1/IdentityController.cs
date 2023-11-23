using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Contracts.Identity;
using SocialMedia.Api.Extensions;
using SocialMedia.Api.Filters;
using SocialMedia.Application.Identity.Commands;

namespace SocialMedia.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.baseRoute)]
[ApiController]
public class IdentityController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public IdentityController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.Registration)]
    [ValideModel]
    public async Task<IActionResult> Register([FromBody] UserRegistrationContract registrationContract)
    {
        var command = _mapper.Map<RegisterCommand>(registrationContract);
        var result = await _mediator.Send(command);

        if(result.IsError)
        {
            return HandleErrorResponse(result.Errors);
        }
        return Ok();
    }

    [HttpPost]
    [Route(ApiRoutes.Identity.login)]
    [ValideModel]
    public async Task<IActionResult>Login([FromBody] LoginContract login)
    {
        var command = _mapper.Map<LoginCommand>(login);
        var result = await _mediator.Send(command);
        if(result.IsError)
        {
            return HandleErrorResponse(result.Errors);
        }
        var authResult = new AuthenticationResult()
        {
            Token = result.Payload
        };
        return Ok(authResult);
    }
    
    [HttpDelete]
    [Route(ApiRoutes.Identity.IdentityById)]
    [ValidGuid("identityUserId")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteAccount(string identityUserId) 
    {
        var identityUserGuid = Guid.Parse(identityUserId);
        var requestorGuid = HttpContext.GetIdentityIdClaimValue();

        var command = new RemoveAccountCommand()
        {
            IdentityUserId = identityUserGuid,
            RequestorGuid = requestorGuid
        };

        var response = await _mediator.Send(command);
        return response.IsError ? HandleErrorResponse(response.Errors) : NoContent();
    }
}
