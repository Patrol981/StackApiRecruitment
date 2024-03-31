using Dapper;

namespace StackAPI.Database;
public class DatabaseInitializer {
  private readonly IDbConnectionFactory _connectionFactory;

  public DatabaseInitializer(IDbConnectionFactory connectionFactory) {
    _connectionFactory = connectionFactory;
  }

  public async Task InitializeAsync() {
    using var conn = await _connectionFactory.CreateConnectionAsync();
    await conn.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Tags (
      Id char(36) PRIMARY KEY,
      HasSynonyms bool DEFAULT FALSE,
      IsModeratorOnly bool DEFAULT FALSE,
      IsRequired bool DEFAULT FALSE,
      Count bigint,
      Name text
    )");
  }

  public async Task DropAsync() {
    using var conn = await _connectionFactory.CreateConnectionAsync();
    await conn.ExecuteAsync(@"DROP TABLE IF EXISTS Tags");
  }
}
