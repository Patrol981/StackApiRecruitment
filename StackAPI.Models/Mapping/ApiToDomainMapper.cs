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

  public static IEnumerable<Tag> ToTag(this IEnumerable<CreateTagRequest> createTagsRequest) {
    return createTagsRequest.Select(x => new Tag {
      Id = TagId.From(Guid.NewGuid()),
      HasSynonyms = x.HasSynonyms,
      IsModeratorOnly = x.IsModeratorOnly,
      IsRequired = x.IsRequired,
      Count = TagCount.From(x.Count),
      Name = TagName.From(x.Name),
    });
  }

  public static TagMeta ToTagMeta(this GetTagSortedRequest getTagSortedRequest) {
    return new TagMeta {
      Offset = getTagSortedRequest.Offset,
      TagOrderBy = TagSortType.From(
        getTagSortedRequest.SortBy ?? nameof(Tag.Name)
      ),
      TagOrderDirection = TagSortDirection.From(
        getTagSortedRequest.SortDirection ?? OrderDirection.DESC.ToString()
      ),
    };
  }

  public static TagName ToTagName(this GetTagNamedRequest getTagTagNamedRequest) {
    var tagName = TagName.From(getTagTagNamedRequest.Name);
    return tagName;
  }
}
