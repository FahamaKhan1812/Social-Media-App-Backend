using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Contracts.Identity;
public class LoginContract
{
    [EmailAddress]
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}