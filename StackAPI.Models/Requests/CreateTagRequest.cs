namespace StackAPI.Models.Requests;
public class CreateTagRequest {
  public bool HasSynonyms { get; init; }
  public bool IsModeratorOnly { get; init; }
  public bool IsRequired { get; init; }
  public decimal Count { get; init; }
  public string Name { get; init; } = default!;
}
