using System.Security.Claims;

namespace SocialMedia.Api.Extensions;
public static class HttpContextExtensions
{
    public static Guid GetUserProfileIdClaimValue(this HttpContext context)
    {
        var userProfileId = GetClaimValue<Guid>(context, "UserProfileId");
        return userProfileId;
    }

    public static Guid GetIdentityIdClaimValue(this HttpContext context)
    {
        var identityId = GetClaimValue<Guid>(context, "IdentityId");
        return identityId;
    }

    public static string GetUserEmailClaimValue(this HttpContext context)
    {
        var userEmail = GetClaimValue<string>(context, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
        return userEmail;
    }

    private static T? GetClaimValue<T>(this HttpContext context, string key)
    {
        var identity = context.User.Identity as ClaimsIdentity;
        var claimValue = identity?.FindFirst(key)?.Value;

        if (typeof(T) == typeof(Guid))
        {
            return claimValue != null ? (T)Convert.ChangeType(Guid.Parse(claimValue), typeof(T)) : default;
        }
        else
        {
            return claimValue != null ? (T)Convert.ChangeType(claimValue, typeof(T)) : default;
        }
    }
}
