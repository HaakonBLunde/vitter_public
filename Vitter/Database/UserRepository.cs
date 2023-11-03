using Vitter.Models;

namespace Vitter.Database;

public class UserRepository : DatabaseConnection
{
    private const string DefaultPhotoUri =
        "https://seccdn.libravatar.org/avatar/40f8d096a3777232204cb3f796c577b7?s=80&forcedefault=y&default=retro";

    public async Task<User?> GetUser(long id)
    {
        await using var conn = await GetOpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT UserId,Username,Email,Password,About,IsAdmin,PhotoUri FROM users WHERE UserId=$id";
        cmd.Parameters.AddWithValue("$id", id);

        User? u = null;

        await using var reader = await cmd.ExecuteReaderAsync();
        while (reader.Read())
        {
            u = new User
            {
                UserId = reader.GetInt64(0),
                Username = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3),
                About = reader.IsDBNull(4) ? null : reader.GetString(4),
                IsAdmin = reader.GetBoolean(5),
                PhotoUri = reader.IsDBNull(6) ? DefaultPhotoUri : reader.GetString(6)
            };
        }

        return u;
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        await using var conn = await GetOpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText =
            "SELECT UserId,Username,Email,Password,About,IsAdmin,PhotoUri FROM users WHERE Username=$username";
        cmd.Parameters.AddWithValue("$username", username);

        User? u = null;

        await using var reader = await cmd.ExecuteReaderAsync();
        while (reader.Read())
        {
            u = new User
            {
                UserId = reader.GetInt64(0),
                Username = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3),
                About = reader.IsDBNull(4) ? null : reader.GetString(4),
                IsAdmin = reader.GetBoolean(5),
                PhotoUri = reader.IsDBNull(6) ? DefaultPhotoUri : reader.GetString(6)
            };
        }

        return u;
    }

    public async Task<User?> GetUserByCredentials(string username, string password)
    {
        await using var conn = await GetOpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText =
            "SELECT UserId,Username,Email,Password,About,IsAdmin,PhotoUri FROM users WHERE Username=$username and Password=$password";
        cmd.Parameters.AddWithValue("$username", username);
        cmd.Parameters.AddWithValue("$password", password);

        User? u = null;

        await using var reader = await cmd.ExecuteReaderAsync();
        while (reader.Read())
        {
            u = new User
            {
                UserId = reader.GetInt64(0),
                Username = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3),
                About = reader.IsDBNull(4) ? null : reader.GetString(4),
                IsAdmin = reader.GetBoolean(5),
                PhotoUri = reader.IsDBNull(6) ? DefaultPhotoUri : reader.GetString(6)
            };
        }

        return u;
    }

    public async Task<long> CreateUser(string username, string password, string email)
    {
        await using var conn = await GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText =
            """
                INSERT INTO users(username, password, email)
                            values ($username, $password, $email);
            """;

        cmd.Parameters.AddWithValue("$username", username);
        cmd.Parameters.AddWithValue("$password", password);
        cmd.Parameters.AddWithValue("$email", email);

        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText =
            "SELECT last_insert_rowid();";

        var newId = (long)(await cmd.ExecuteScalarAsync())!;

        return newId;
    }

    public async Task UpdateUser(long id, string about, string photoUri)
    {
        await using var conn = await GetOpenConnection();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE users SET about=$about, photoUri=$photouri WHERE userId=$userid;";

        cmd.Parameters.AddWithValue("$about", about ?? "");
        cmd.Parameters.AddWithValue("$photouri", photoUri ?? "");
        cmd.Parameters.AddWithValue("$userid", id);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<bool> UserExists(string email)
    {
        await using var conn = await GetOpenConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText =
            "SELECT EXISTS(SELECT 1 FROM users WHERE Email=$email)";
        cmd.Parameters.AddWithValue("$email", email);

        var exists = (long)(await cmd.ExecuteScalarAsync());

        return exists == 1;
    }
}