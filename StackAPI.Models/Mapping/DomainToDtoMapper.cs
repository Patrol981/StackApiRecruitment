using StackAPI.Models.Data;
using StackAPI.Models.Domain;

namespace StackAPI.Models.Mapping;
public static class DomainToDtoMapper {
  public static TagDto ToTagDto(this Tag tag) {
    return new TagDto() {
      Id = tag.Id.Value.ToString(),
      HasSynonyms = tag.HasSynonyms,
      IsModeratorOnly = tag.IsModeratorOnly,
      IsRequired = tag.IsRequired,
      Count = tag.Count.Value,
      Name = tag.Name.Value,
    };
  }
}
