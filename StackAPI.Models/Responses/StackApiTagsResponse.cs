namespace StackAPI.Models.Responses;
public class StackApiTagsResponse {
  public IEnumerable<StackApiTag> Items { get; init; } = Enumerable.Empty<StackApiTag>();
}
