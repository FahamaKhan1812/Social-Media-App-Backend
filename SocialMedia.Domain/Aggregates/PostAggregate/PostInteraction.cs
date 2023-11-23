using SocialMedia.Domain.Aggregates.UserProfileAggregate;

namespace SocialMedia.Domain.Aggregates.PostAggregate;
public class PostInteraction
{
    private PostInteraction()
    {        
    }
    public Guid InteractionId { get; private set; }
    public Guid PostId { get; private set; }
    public Guid? UserProfileId { get; private set; }
    public UserProfile UserProfile { get; set; }
    public InteractionType InteractionType { get; private set; }

    public static PostInteraction CreatePostInteraction(Guid postId, InteractionType type, Guid userProfileId) 
    {
        return new PostInteraction
        {
            PostId = postId,
            UserProfileId = userProfileId,
            InteractionType = type
        };
    }
}
