using SocialMedia.Domain.Aggregates.PostAggregate;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Api.Contracts.Posts.Requests;

public class PostInteractionCreate
{
    [Required]
    public InteractionType Type { get; set; }
}
