using System.Data;
using Vitter.Models;

namespace Vitter.Database;

public class PostRepository : DatabaseConnection
{
    private const string DefaultPhotoUri =
        "https://seccdn.libravatar.org/avatar/40f8d096a3777232204cb3f796c577b7?s=80&forcedefault=y&default=retro";

    public async Task<List<PostInfoDTO>> GetLatestPosts(long? userId = null)
    {
        await using var conn = await GetOpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText =
            "SELECT p.PostId,p.Content,p.UserId,u.Username,p.CreatedAt,u.PhotoUri FROM posts p JOIN users u on p.UserId = u.UserId WHERE 1=1";

        if (userId.HasValue)
        {
            cmd.CommandText += " AND p.userId=" + userId;
        }

        cmd.CommandText += " ORDER BY p.CreatedAt DESC";

        List<PostInfoDTO> posts = new();

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var p = new PostInfoDTO()
            {
                PostId = reader.GetInt64(0),
                Content = reader.GetString(1),
                UserId = reader.GetInt64(2),
                Username = reader.GetString(3),
                CreatedAt = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64(4)),
                PhotoUri = reader.IsDBNull(5) ? DefaultPhotoUri : reader.GetString(5)
            };

            posts.Add(p);
        }

        return posts;
    }

    public async Task<long> CreatePost(long userId, string content)
    {
        await using var conn = await GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText =
            """
                INSERT INTO posts(Content, UserId, CreatedAt)
                            values ($content, $userid, $createdat);
            """;

        cmd.Parameters.AddWithValue("$content", content);
        cmd.Parameters.AddWithValue("$userid", userId);
        cmd.Parameters.AddWithValue("$createdat", DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = "SELECT last_insert_rowid();";

        var newId = (long)(await cmd.ExecuteScalarAsync())!;

        return newId;
    }

    public async Task<List<PostInfoDTO>> SearchPosts(string query)
    {
        await using var conn = await GetOpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText =
            "SELECT p.PostId,p.Content,p.UserId,u.Username,p.CreatedAt,u.PhotoUri FROM posts p JOIN users u on p.UserId = u.UserId WHERE p.Content LIKE '%" +
            query + "%'";

        List<PostInfoDTO> posts = new();

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var p = new PostInfoDTO()
            {
                PostId = reader.GetInt64(0),
                Content = reader.GetString(1),
                UserId = reader.GetInt64(2),
                Username = reader.GetString(3),
                CreatedAt = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64(4)),
                PhotoUri = reader.IsDBNull(5) ? DefaultPhotoUri : reader.GetString(5)
            };

            posts.Add(p);
        }

        return posts;
    }
}