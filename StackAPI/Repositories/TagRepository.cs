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

  public async Task<bool> CreateManyAsync(IList<TagDto> tags) {
    using var conn = await _connectionFactory.CreateConnectionAsync();
    var result = await conn.ExecuteAsync(
     @"INSERT INTO Tags (Id, HasSynonyms, IsModeratorOnly, IsRequired, Count, Name)
      VALUES (@Id, @HasSynonyms, @IsModeratorOnly, @IsRequired, @Count, @Name)",
     tags
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

  public async Task<IEnumerable<TagDto>> GetManyAsync(IList<Guid> ids) {
    using var conn = await _connectionFactory.CreateConnectionAsync();
    return await conn.QueryAsync<TagDto>(
      "SELECT * FROM Tags WHERE Id = ANY(@Ids)",
      new { Ids = ids }
    );
  }

  public async Task<IEnumerable<TagDto>> GetManyAsync(IList<string> names) {
    using var conn = await _connectionFactory.CreateConnectionAsync();
    return await conn.QueryAsync<TagDto>(
      "SELECT * FROM Tags WHERE Name = ANY(@Names)",
      new { Names = names }
    );
  }

  public async Task<IEnumerable<TagDto>> GetAllAsync() {
    using var conn = await _connectionFactory.CreateConnectionAsync();
    return await conn.QueryAsync<TagDto>("SELECT * FROM Tags");
  }

  public async Task<IEnumerable<TagDto>> GetAllAsync(TagDtoMeta tagDtoMeta) {
    // using builder here to avoid string interpolation and SQL Injection
    var builder = new SqlBuilder();
    builder.AddParameters(new { limit = tagDtoMeta.Limit });
    builder.AddParameters(new { offset = tagDtoMeta.Offset });
    builder.OrderBy(string.Format(
      "{0} {1}",
      tagDtoMeta.TagOrderBy,
      tagDtoMeta.TagOrderDirection == "DESC" ? "DESC" : "ASC"
    ));
    var query = builder.AddTemplate("SELECT * FROM Tags /**orderby**/ LIMIT @limit OFFSET @offset");
    using var conn = await _connectionFactory.CreateConnectionAsync();
    return await conn.QueryAsync<TagDto>(query.RawSql, query.Parameters);
  }

  public async Task<bool> DeleteAsync(Guid id) {
    using var conn = await _connectionFactory.CreateConnectionAsync();
    var result = await conn.ExecuteAsync(
      @"DELETE FROM Tags WHERE Id = @Id",
      new { Id = id.ToString() }
    );
    return result > 0;
  }

  public async Task<bool> DeleteManyAsync(IList<Guid> ids) {
    using var conn = await _connectionFactory.CreateConnectionAsync();
    var result = await conn.ExecuteAsync(
      @"DELETE FROM Tags WHERE Id = ANY(@Ids)",
      new { Ids = ids.Select(id => id.ToString()).ToList() }
    );
    return result > 0;
  }

  public async Task<TagSumCountDto> GetTotalCountAsync() {
    using var conn = await _connectionFactory.CreateConnectionAsync();
    return await conn.QuerySingleAsync<TagSumCountDto>(
      @"SELECT SUM(Count) AS TotalCount FROM Tags LIMIT 1"
    );
  }

  public async Task<TagsTotalDto> GetRecordsNumber() {
    // Not calculating it along the way with GetAllAsync to keep single
    // resposibility principle intact, if I didn't care about this
    // I would probably put it together to optimize queries
    using var conn = await _connectionFactory.CreateConnectionAsync();
    return await conn.QuerySingleAsync<TagsTotalDto>(
      @"SELECT COUNT(Id) AS TotalCount FROM Tags LIMIT 1"
    );
  }
}
