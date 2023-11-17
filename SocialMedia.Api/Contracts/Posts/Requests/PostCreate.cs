using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Contracts.Posts.Requests;
public class PostCreate
{
    [Required]
    public string TextContent { get; set; }
}
