namespace StackAPI.Models.Requests;
public class GetTagSortedRequest {
  public string SortBy { get; init; } = default!;
  public string SortDirection { get; init; } = default!;
  public int Offset { get; init; } = default!;
}
