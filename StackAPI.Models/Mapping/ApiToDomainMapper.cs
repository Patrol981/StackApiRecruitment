using StackAPI.Models.Domain;
using StackAPI.Models.Domain.Common;
using StackAPI.Models.Requests;

namespace StackAPI.Models.Mapping;
public static class ApiToDomainMapper {
  public static Tag ToTag(this CreateTagRequest createTagRequest) {
    return new Tag {
      Id = TagId.From(Guid.NewGuid()),
      HasSynonyms = createTagRequest.HasSynonyms,
      IsModeratorOnly = createTagRequest.IsModeratorOnly,
      IsRequired = createTagRequest.IsRequired,
      Count = TagCount.From(createTagRequest.Count),
      Name = TagName.From(createTagRequest.Name),
    };
  }
}
