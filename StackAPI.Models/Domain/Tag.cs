using StackAPI.Models.Domain.Common;

namespace StackAPI.Models.Domain;
public class Tag {
  public TagId Id { get; init; } = TagId.From(Guid.NewGuid());
  public bool HasSynonyms { get; init; }
  public bool IsModeratorOnly { get; init; }
  public bool IsRequired { get; init; }
  public TagCount Count { get; init; } = default!;
  public TagName Name { get; init; } = default!;
}
