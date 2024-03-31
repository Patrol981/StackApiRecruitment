namespace StackAPI.Models.Data;
public class TagDtoMeta {
  public string TagOrderBy { get; init; } = default!;
  public string TagOrderDirection { get; init; } = default!;
  public int Limit { get; init; } = default!;
  public int Offset { get; init; } = default!;
}
