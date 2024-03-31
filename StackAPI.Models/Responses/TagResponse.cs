namespace StackAPI.Models.Responses;
public class TagResponse {
  public Guid Id { get; init; }
  public bool HasSynonyms { get; init; }
  public bool IsModeratorOnly { get; init; }
  public bool IsRequired { get; init; }
  public long Count { get; init; }
  public string Name { get; init; } = default!;
}