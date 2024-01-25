using MediatR;
using SocialMedia.Application.Models;
using System.Security.Claims;

namespace SocialMedia.Application.Identity.Queries;
public class GetCurrentUser : IRequest<OperationResult<Domain.Aggregates.UserProfileAggregate.UserProfile>>
{
    public Guid UserProfileId { get; set; }
    public string UserEmail { get; set; }
}
