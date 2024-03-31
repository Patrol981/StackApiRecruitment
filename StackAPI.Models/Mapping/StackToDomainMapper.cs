using StackAPI.Models.Domain;
using StackAPI.Models.Domain.Common;
using StackAPI.Models.Responses;

namespace StackAPI.Models.Mapping;
public static class StackToDomainMapper {
  public static IEnumerable<Tag> ToTag(this StackApiTagsResponse stackResponse) {
    return stackResponse.Items.Select(x => new Tag {
      Id = TagId.From(Guid.NewGuid()),
      HasSynonyms = x.HasSynonyms,
      IsModeratorOnly = x.IsModeratorOnly,
      IsRequired = x.IsRequired,
      Count = TagCount.From(x.Count),
      Name = TagName.From(x.Name),
    });
  }
}
