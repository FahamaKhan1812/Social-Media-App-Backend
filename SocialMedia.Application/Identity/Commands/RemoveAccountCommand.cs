using MediatR;
using SocialMedia.Application.Models;

namespace SocialMedia.Application.Identity.Commands;
public class RemoveAccountCommand : IRequest<OperationResult<bool>>
{
    public Guid IdentityUserId { get; set; }
    public Guid RequestorGuid { get; set; }
}
