namespace Vitter.Utils;

public static class SignInExtensions
{
    private const string SessionCookieName = "SigninSession";

    // Extenstion method to easily check if request is authenticated
    public static bool IsAuthenticated(this HttpContext context)
    {
        var identifier = context.Request.Cookies[SessionCookieName];

        if (string.IsNullOrEmpty(identifier))
        {
            return false;
        }

        var data = SignInManager.DecodeSession(identifier);

        if (data == null)
        {
            return false;
        }

        if (DateTimeOffset.UtcNow > data.CreatedAt.AddDays(1))
        {
            return false;
        }

        return true;
    }

    public static SessionData? GetSessionData(this HttpContext context)
    {
        var identifier = context.Request.Cookies[SessionCookieName];

        if (string.IsNullOrEmpty(identifier))
        {
            return null;
        }

        var data = SignInManager.DecodeSession(identifier);

        if (data == null)
        {
            return null;
        }

        if (DateTimeOffset.UtcNow > data.CreatedAt.AddDays(1))
        {
            return null;
        }

        return data;
    }
}