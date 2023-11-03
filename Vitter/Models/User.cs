namespace Vitter.Models;

public class User
{
    public long UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? About { get; set; }
    public bool IsAdmin { get; set; }
    public string? PhotoUri { get; set; }
}