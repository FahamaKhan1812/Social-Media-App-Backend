using SocialMedia.Domain.Exceptions;
using SocialMedia.Domain.Validators.PostValidators;

namespace SocialMedia.Domain.Aggregates.PostAggregate;
public class PostComment
{
    private PostComment()
    {            
    }
    public Guid CommentId { get; private set; }
    public Guid PostId { get; private set; }
    public string Text { get; private set; }
    public Guid UserProfileId { get; private set; }
    public DateTime DateCreated { get; private set; }
    public DateTime LastModified { get; private set;}

    //Factories
    /// <summary>
    /// Creates a post comment
    /// </summary>
    /// <param name="postId">The ID of the post to which the comment belongs</param>
    /// <param name="text">Text content of the comment</param>
    /// <param name="userProfileId">The ID of the user who created the comment</param>
    /// <returns><see cref="PostComment"/></returns>
    /// <exception cref="PostCommentNotValidException">Thrown if the data provided for the post comment
    /// is not valid</exception>
    public static PostComment CreatePostComment(Guid postId, string text, Guid userProfileId)
    {
        PostCommentValidator validator = new();

        PostComment objToValidate = new() 
        { 
            PostId = postId, 
            Text = text, 
            UserProfileId = userProfileId,
            DateCreated = DateTime.Now,
            LastModified = DateTime.Now
        };

        var validationResult = validator.Validate(objToValidate);
        if (validationResult.IsValid)
        {
            return objToValidate;
        }
        PostCommentNotValidException exception = new("Post comment is not valid");
        foreach (var error in validationResult.Errors)
        {
            exception.ValidationErrors.Add(error.ErrorMessage);
        }
        throw exception;
    }

    //public methods

    public void UpdateCommentText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            PostCommentNotValidException exception = new("Can not update post. The post text is not valid");
            exception.ValidationErrors.Add("The provided text is either null or white space");
            throw exception;
        }
        Text = text;
        LastModified = DateTime.UtcNow;
    }
}