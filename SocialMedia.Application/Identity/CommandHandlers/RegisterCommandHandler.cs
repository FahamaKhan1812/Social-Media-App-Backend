using MediatR;
using Microsoft.AspNetCore.Identity;
using SocialMedia.Application.Enums;
using SocialMedia.Application.Identity.Commands;
using SocialMedia.Application.Models;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.UserProfileAggregate;
using SocialMedia.Domain.Exceptions;

namespace SocialMedia.Application.Identity.CommandHandlers;
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, OperationResult<string>>
{
    private readonly DataContext _dataContext;
    private readonly UserManager<IdentityUser> _userManager;

    public RegisterCommandHandler(DataContext dataContext, UserManager<IdentityUser> userManager)
    {
        _dataContext = dataContext;
        _userManager = userManager;
    }

    public async Task<OperationResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
       var result = new OperationResult<string>();
        try
        {
            var existingIdentity = await _userManager.FindByEmailAsync(request.Username);

            if(existingIdentity!= null)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.IdentityUserAlreadyExists,
                    Message = "Provided informations is already exists."
                };
                result.Errors.Add(error);
                return result;
            }
  
            IdentityUser identity = new()
            {
                Email = request.Username,
                UserName = request.Username
            };

            //creating transaction
            using var transaction =  _dataContext.Database.BeginTransaction();
            
            var createdIdentity = await _userManager.CreateAsync(identity, request.Password);
            if(!createdIdentity.Succeeded)
            {
                await transaction.RollbackAsync(cancellationToken);
                result.IsError = true;
                foreach (var identityErrors in createdIdentity.Errors)
                {
                    Error error = new()
                    {
                        Code = ErrorCode.IdentityCreationFailed,
                        Message = identityErrors.Description
                    };
                    result.Errors.Add(error);
                }
                return result;
            }
            var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.Username, request.Phone, request.DateOfBirth, request.CurrentCity);

            var profile =  Domain.Aggregates.UserProfileAggregate.UserProfile.CreateUserProfile(identity.Id, profileInfo);
            try
            {
                _dataContext.Add(profile);
                await _dataContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            result.Payload = "User is Created Successfully";
        }
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
}
