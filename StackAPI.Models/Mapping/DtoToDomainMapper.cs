using StackAPI.Models.Data;
using StackAPI.Models.Domain;
using StackAPI.Models.Domain.Common;

namespace StackAPI.Models.Mapping;
public static class DtoToDomainMapper {
  public static Tag ToTag(this TagDto tagDto) {
    return new Tag {
      Id = TagId.From(Guid.Parse(tagDto.Id)),
      HasSynonyms = tagDto.HasSynonyms,
      IsModeratorOnly = tagDto.IsModeratorOnly,
      IsRequired = tagDto.IsRequired,
      Count = TagCount.From(tagDto.Count),
      Name = TagName.From(tagDto.Name),
    };
  }

  public static TagsTotal ToTagsTotal(this TagsTotalDto tagsTotalDto) {
    return new TagsTotal {
      TotalCount = tagsTotalDto.TotalCount,
    };
  }
}
