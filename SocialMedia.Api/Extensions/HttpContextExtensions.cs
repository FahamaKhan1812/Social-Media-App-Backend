using System.Security.Claims;

namespace SocialMedia.Api.Extensions;
public static class HttpContextExtensions
{ 
    public static Guid GetUserProfileIdClaimValue(this HttpContext context)
    {
        var userProfileId =  GetClaimValue(context, "UserProfileId");
        return userProfileId;
    }
    public static Guid GetIdentityIdClaimValue(this HttpContext context)
    {
        var identityId = GetClaimValue(context, "IdentityId");
        return identityId;
    }

    private static Guid GetClaimValue(this HttpContext context,string key) 
    {
        var identity = context.User.Identity as ClaimsIdentity;
        return Guid.Parse(identity?.FindFirst(key)?.Value);
    }
}
