using Microsoft.Data.Sqlite;

namespace Vitter.Database;

public abstract class DatabaseConnection
{
    private const string ConnectionString = "Data Source=Vitter.db";

    internal static async Task<SqliteConnection> GetOpenConnection()
    {
        var connection = new SqliteConnection(ConnectionString);
        await connection.OpenAsync();

        return connection;
    }
}