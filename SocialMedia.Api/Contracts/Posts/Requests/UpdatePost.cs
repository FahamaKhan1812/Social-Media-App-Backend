using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Contracts.Posts.Requests;
public class UpdatePost 
{
    [Required]
    public string Text { get; set; }
}
