using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Identity.Commands;
using SocialMedia.Application.Models;
using SocialMedia.Application.Services;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.UserProfileAggregate;
using SocialMedia.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialMedia.Application.Identity.CommandHandlers;
public class LoginCommandHandler : IRequestHandler<LoginCommand, OperationResult<string>>
{
    private readonly DataContext _dataContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityServices _identityServices;


    public LoginCommandHandler(DataContext dataContext, UserManager<IdentityUser> userManager, IdentityServices identityServices)
    {
        _dataContext = dataContext;
        _userManager = userManager;
        _identityServices = identityServices;
    }

    public async Task<OperationResult<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();
        try
        {
            var identityUser = await _userManager.FindByEmailAsync(request.Username);
            if (identityUser == null)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.IdentityUserNotFound,
                    Message = "Invalid Credentials."
                };
                result.Errors.Add(error);
                return result;
            }

            var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);
            if (!validPassword)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.IncorrectPassword,
                    Message = "Invalid Credentials."
                };
                result.Errors.Add(error);
                return result;
            }

            var userProfile = await _dataContext.UserProfiles.FirstOrDefaultAsync(up => up.IdentityId == identityUser.Id, cancellationToken);
            result.Payload = GetJwtString(identityUser, userProfile);    
           
;        }
        catch (UserProfileNotValidException e)
        {
            result.IsError = true;
            e.ValidationErrors.ForEach(err =>
            {
                var errors = new Error
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

    private string GetJwtString(IdentityUser identityUser, Domain.Aggregates.UserProfileAggregate.UserProfile userProfile)
    {
        var claimsIdentity = new ClaimsIdentity(new Claim[]
           {
                    new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
               new Claim("IdentityId", identityUser.Id),
                    new Claim("UserProfileId", userProfile.UserProfileId.ToString()),
           });
        var token = _identityServices.CreateSecurityToken(claimsIdentity);
      return _identityServices.WriteToken(token);
    }
}
