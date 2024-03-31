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

  public static IEnumerable<TagDto> ToTagDto(this IEnumerable<Tag> tags) {
    return tags.Select(x => new TagDto {
      Id = x.Id.Value.ToString(),
      HasSynonyms = x.HasSynonyms,
      IsModeratorOnly = x.IsModeratorOnly,
      IsRequired = x.IsRequired,
      Count = x.Count.Value,
      Name = x.Name.Value,
    });
  }

  public static TagDtoMeta ToTagDtoMeta(this TagMeta tagMeta) {
    return new TagDtoMeta() {
      Limit = tagMeta.Limit,
      Offset = tagMeta.Offset,
      TagOrderBy = tagMeta.TagOrderBy.Value,
      TagOrderDirection = tagMeta.TagOrderDirection.Value,
    };
  }
}
