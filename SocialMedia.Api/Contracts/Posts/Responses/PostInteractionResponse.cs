using SocialMedia.Domain.Aggregates.PostAggregate;

namespace SocialMedia.Api.Contracts.Posts.Responses;
public class PostInteractionResponse
{
    public Guid InteractionId { get; set; }
    public string Type { get; set; }
    public InteractionUser Author { get; set; }
}
