using MediatR;
using SocialMedia.Application.Models;
using SocialMedia.Application.UserProfile.Commands;
using SocialMedia.Dal.Data;
using SocialMedia.Domain.Aggregates.UserProfileAggregate;

namespace SocialMedia.Application.UserProfile.CommandHandlers;
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>>
{
    private readonly DataContext _dataContext;
    public CreateUserCommandHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>();

        BasicInfo basicInfo = BasicInfo.CreateBasicInfo(
            request.FirstName, 
            request.LastName, 
            request.EmailAddress,
            request.Phone,
            request.DateOfBirth, 
            request.CurrentCity);

        var userProfile = Domain.Aggregates.UserProfileAggregate.UserProfile.CreateUserProfile(Guid.NewGuid().ToString(), basicInfo);
        await _dataContext.UserProfiles.AddAsync(userProfile, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
        result.Payload = userProfile;
        return result;

    }
}
