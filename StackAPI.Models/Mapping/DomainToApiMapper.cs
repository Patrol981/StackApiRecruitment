using StackAPI.Models.Domain;
using StackAPI.Models.Responses;

namespace StackAPI.Models.Mapping;
public static class DomainToApiMapper {
  public static TagResponse ToTagResponse(this Tag tag) {
    return new TagResponse {
      Id = tag.Id.Value,
      HasSynonyms = tag.HasSynonyms,
      IsModeratorOnly = tag.IsModeratorOnly,
      IsRequired = tag.IsRequired,
      Count = tag.Count.Value,
      Name = tag.Name.Value,
    };
  }

  public static GetAllTagsResponse ToTagsResponse(this IEnumerable<Tag> tags) {
    return new GetAllTagsResponse {
      Tags = tags.Select(x => new TagResponse {
        Id = x.Id.Value,
        HasSynonyms = x.HasSynonyms,
        IsModeratorOnly = x.IsModeratorOnly,
        IsRequired = x.IsRequired,
        Count = x.Count.Value,
        Name = x.Name.Value,
      })
    };
  }
}
