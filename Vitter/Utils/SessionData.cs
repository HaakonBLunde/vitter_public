namespace Vitter.Utils;

public class SessionData
{
    public long UserId { get; set; }
    public bool IsAdmin { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}