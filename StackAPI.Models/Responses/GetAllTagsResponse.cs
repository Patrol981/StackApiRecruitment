namespace StackAPI.Models.Responses;
public class GetAllTagsResponse {
  public IEnumerable<TagResponse> Tags { get; init; } = Enumerable.Empty<TagResponse>();
}
