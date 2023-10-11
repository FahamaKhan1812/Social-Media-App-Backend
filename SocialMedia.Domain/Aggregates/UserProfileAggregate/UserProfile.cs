namespace SocialMedia.Domain.Aggregates.UserProfileAggregate;
public class UserProfile
{
    private UserProfile()
    {
            
    }
    public Guid UserProfileId { get; private set; }
    
    //forieng key
    public string? IdentityId { get; private set; }
    
    // One-To-One Relation
    public BasicInfo BasicInfo { get; private set; }
    public DateTime DateCreated { get; private set; }
    public DateTime LastModified { get; private set; }
    
    //Factory method
    public static UserProfile CreateUserProfile(string identityId, BasicInfo basicInfo)
    {
        // TODO: add validation, error handling strategies, error notification strategies
        UserProfile userProfile = new()
        {
            IdentityId = identityId,
            BasicInfo = basicInfo,
            DateCreated = DateTime.UtcNow,
            LastModified = DateTime.UtcNow

        };    
        return userProfile;
    }

    public void UpdateBasicInfo(BasicInfo newInfo)
    {
        BasicInfo = newInfo;
        LastModified = DateTime.UtcNow;
    }
}