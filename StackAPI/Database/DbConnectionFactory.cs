using System.Data;

namespace StackAPI.Database;

public interface IDbConnectionFactory {
  public Task<IDbConnection> CreateConnectionAsync();
}

public class PostgreSQLConnectionFactory : IDbConnectionFactory {
  private readonly string _connectionString;

  public PostgreSQLConnectionFactory(string connectionString) {
    _connectionString = connectionString;
  }

  public async Task<IDbConnection> CreateConnectionAsync() {
    var conn = new Npgsql.NpgsqlConnection(_connectionString);
    await conn.OpenAsync();
    return conn;
  }
}
