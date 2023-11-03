namespace Vitter.Models;

public class Post
{
    public long PostId { get; set; }
    public string Content { get; set; }
    public long UserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}