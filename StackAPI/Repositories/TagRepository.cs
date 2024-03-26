using Dapper;

using StackAPI.Database;
using StackAPI.Models.Data;

namespace StackAPI.Repositories;

public class TagRepository : ITagRepository {
  private readonly IDbConnectionFactory _connectionFactory;

  public TagRepository(IDbConnectionFactory connectionFactory) {
    _connectionFactory = connectionFactory;
  }

  public async Task<bool> CreateAsync(TagDto tag) {
    using var conn = await _connectionFactory.CreateConnectionAsync();
    var result = await conn.ExecuteAsync(
      @"INSERT INTO Tags (Id, HasSynonyms, IsModeratorOnly, IsRequired, Count, Name)
      VALUES (@Id, @HasSynonyms, @IsModeratorOnly, @IsRequired, @Count, @Name)",
      tag
    );
    return result > 0;
  }

  public async Task<TagDto?> GetAsync(Guid id) {
    using var conn = await _connectionFactory.CreateConnectionAsync();
    return await conn.QuerySingleOrDefaultAsync<TagDto>(
      "SELECT * FROM Tags WHERE Id = @Id LIMIT 1",
      new { Id = id.ToString() }
    );
  }

  public async Task<TagDto?> GetAsync(string name) {
    using var conn = await _connectionFactory.CreateConnectionAsync();
    return await conn.QuerySingleOrDefaultAsync<TagDto>(
      "SELECT * FROM Tags WHERE Name = @Name LIMIT 1",
      new { Name = name }
    );
  }

  public async Task<IEnumerable<TagDto>> GetAllAsync() {
    using var conn = await _connectionFactory.CreateConnectionAsync();
    return await conn.QueryAsync<TagDto>("SELECT * FROM Tags");
  }

  public async Task<bool> DeleteAsync(Guid id) {
    using var conn = await _connectionFactory.CreateConnectionAsync();
    var result = await conn.ExecuteAsync(
      @"DELETE FROM Tags WHERE Id = @Id",
      new { Id = id.ToString() }
    );
    return result > 0;
  }
}
