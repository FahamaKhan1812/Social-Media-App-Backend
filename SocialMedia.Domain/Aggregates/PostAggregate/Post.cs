using SocialMedia.Domain.Aggregates.PostAggregate;
using SocialMedia.Domain.Aggregates.UserProfileAggregate;
using SocialMedia.Domain.Exceptions;
using SocialMedia.Domain.Validators.PostValidators;

namespace SocialMedia.Domain.Aggregates.PostAggregates;
public class Post
{
    private readonly List<PostComment> _comments = new();
    private readonly List<PostInteraction> _interactions = new();
    private Post()
    {
    }

    public Guid PostId { get; private set; }
    public Guid UserProfileId { get; private set; }
    public UserProfile UserProfile { get; private set; }
    public string TextContent { get; private set; }
    public DateTime CreatedDate {  get; private set; }
    public DateTime LastModified { get; private set;}
    public IEnumerable<PostComment> Comments { get { return _comments; } }
    public IEnumerable<PostInteraction> Interactions { get { return _interactions; } }

    // Factories
    /// <summary>
    /// Creates a new post instance
    /// </summary>
    /// <param name="userProfileId">User profile ID</param>
    /// <param name="textContent">Post content</param>
    /// <returns><see cref="Post"/></returns>
    /// <exception cref="PostNotValidException"></exception>
    public static Post CreatePost(Guid userProfileId, string textContent)
    {
        PostValidator validator = new();
        
        Post objToValidate = new()
        {
            UserProfileId = userProfileId,
            TextContent = textContent,
            CreatedDate = DateTime.UtcNow, 
            LastModified = DateTime.UtcNow
        };
        var validationResult = validator.Validate(objToValidate);
        if (validationResult.IsValid)
        {
            return objToValidate;
        }
        PostNotValidException exception = new("Post comment is not valid");
        foreach (var error in validationResult.Errors)
        {
            exception.ValidationErrors.Add(error.ErrorMessage);
        }
        throw exception;
    }

    //public methods
    /// <summary>
    /// Updates the post text
    /// </summary>
    /// <param name="newText">The updated post text</param>
    /// <exception cref="PostNotValidException"></exception>
    public void UpdatePost(string newText)
    {
        if(string.IsNullOrWhiteSpace(newText))
        {
            PostNotValidException exception = new("Can not update post. The post text is not valid");
            exception.ValidationErrors.Add("The provided text is either null or white space");
            throw exception;
        }
        TextContent = newText;
        LastModified = DateTime.UtcNow;
    }
    public void AddPostComment(PostComment postComment) 
    {
        _comments.Add(postComment);
    }
    public void RemovePostComment(PostComment postComment)
    {
        _comments.Remove(postComment);
    }
    public void UpdatePostComment(Guid postCommentId, string updatedComment)
    {
        var comment = _comments.FirstOrDefault(c => c.CommentId == postCommentId);
        if(comment != null && !string.IsNullOrWhiteSpace(updatedComment))
        {
            comment.UpdateCommentText(updatedComment);
        }
    }
    public void AddInteraction(PostInteraction postInteraction)
    {
        _interactions.Add(postInteraction);
    }
    public void RemoveInteraction(PostInteraction postInteraction) 
    { 
        _interactions.Remove(postInteraction); 
    }

}
