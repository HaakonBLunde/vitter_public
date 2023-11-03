namespace Vitter.Models;

public class PostInfoDTO
{
    public long PostId { get; set; }
    public string Content { get; set; }
    public long UserId { get; set; }
    public string Username { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? PhotoUri { get; set; }
}