namespace StackAPI.Models.Data;

public class TagDto {
  public string Id { get; init; } = default!;
  public bool HasSynonyms { get; init; }
  public bool IsModeratorOnly { get; init; }
  public bool IsRequired { get; init; }
  public long Count { get; init; }
  public string Name { get; init; } = default!;
}
