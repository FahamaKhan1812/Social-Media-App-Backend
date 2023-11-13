using MediatR;
using SocialMedia.Application.Models;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Application.Identity.Commands;
public class LoginCommand : IRequest<OperationResult<string>>
{
    [Required]
    [EmailAddress]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
