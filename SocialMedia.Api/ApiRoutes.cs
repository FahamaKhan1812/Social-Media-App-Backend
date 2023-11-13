namespace SocialMedia.Api;
public class ApiRoutes
{
    public const string baseRoute = "/api/v{version:apiVersion}/[controller]";

   public class UserProfiles
    {
        public const string IdRoute = "{id}";
    }
    public class Posts
    {
        public const string IdRoute = "{id}";
        public const string PostComment = "{postId}/comments";
        public const string CommentById = "{postId}/comments/{commentId}";
    }
    public static class Identity
    {
        public const string login = "login";
        public const string Registration = "registration";
    }
}
