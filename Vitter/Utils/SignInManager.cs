using System.Security.Authentication;
using Vitter.Database;

namespace Vitter.Utils;

public class SignInManager
{
    private const string SessionCookieName = "SigninSession";

    private readonly UserRepository _userRepository;

    public SignInManager(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Signs in a user with the given userId
    public async Task SignInUser(long id, HttpContext context)
    {
        var user = await _userRepository.GetUser(id);

        if (user == null)
        {
            throw new ArgumentException("Not a valid id", nameof(id));
        }

        var sessionIdentifier = CreateSession(user.UserId, user.IsAdmin);

        context.Response.Cookies.Append(SessionCookieName, sessionIdentifier, new CookieOptions()
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = true,
            MaxAge = TimeSpan.FromDays(1)
        });
    }

    // Signs in a user with the given user credentials
    public async Task SignInUser(string username, string password, HttpContext context)
    {
        var user = await _userRepository.GetUserByCredentials(username, password);

        if (user == null)
        {
            throw new InvalidCredentialException();
        }

        var sessionIdentifier = CreateSession(user.UserId, user.IsAdmin);

        context.Response.Cookies.Append(SessionCookieName, sessionIdentifier, new CookieOptions()
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Secure = true,
            MaxAge = TimeSpan.FromDays(1)
        });
    }

    public void SignOut(HttpContext context)
    {
        context.Response.Cookies.Delete(SessionCookieName);
    }
    
    

    private static string CreateSession(long id, bool isAdmin)
    {
        var identifier = id.ToString() + '-';
        identifier += isAdmin ? "1-" : "0-";
        identifier += DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        return Base64Encode(identifier);
    }

    public static SessionData? DecodeSession(string sessionId)
    {
        var decodedIdentifier = Base64Decode(sessionId);
        var sessionParts = decodedIdentifier.Split('-');

        if (sessionParts.Length != 3)
        {
            return null;
        }

        var sessionData = new SessionData();

        if (!long.TryParse(sessionParts[0], out long userId))
        {
            return null;
        }

        sessionData.UserId = userId;

        sessionData.IsAdmin = sessionParts[1] == "1";

        if (!long.TryParse(sessionParts[2], out long unixTime))
        {
            return null;
        }

        var sessionCreationTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTime);
        sessionData.CreatedAt = sessionCreationTime;

        return sessionData;
    }

    private static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    private static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
}