using StackAPI.Models.Responses;

namespace StackAPI.Models.Mapping;
public static class StackToApiMapper {
  public static GetAllTagsResponse ToTagsResponse(this StackApiTagsResponse stackResponse) {
    return new GetAllTagsResponse {
      Tags = stackResponse.Items.Select(x => new TagResponse {
        HasSynonyms = x.HasSynonyms,
        IsModeratorOnly = x.IsModeratorOnly,
        IsRequired = x.IsRequired,
        Count = x.Count,
        Name = x.Name,
      })
    };
  }
}
