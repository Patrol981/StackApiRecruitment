namespace StackAPI.Models.Responses;
public class GetAllTagsResponse {
  public long? TotalRecords { get; set; }// Sum from db
  public long? TotalPages { get; set; } // TotalRecords / PageSize
  public long? PageSize { get; set; } // Limit Value
  public long? CurrentPage { get; set; } // Offset Value
  public IEnumerable<TagResponse> Tags { get; init; } = Enumerable.Empty<TagResponse>();
}
