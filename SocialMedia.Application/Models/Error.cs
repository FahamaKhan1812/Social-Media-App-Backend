using SocialMedia.Application.Enums;

namespace SocialMedia.Application.Models;
public class Error
{
    public ErrorCode Code { get; set; }
    public string? Message { get; set; }
}
