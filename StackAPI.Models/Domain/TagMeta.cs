using StackAPI.Models.Domain.Common;

namespace StackAPI.Models.Domain;
public class TagMeta {
  public int Limit { get; init; } = 100;
  public int Offset { get; init; } = 0;
  public TagSortType TagOrderBy { get; init; } = TagSortType.From(nameof(Tag.Name));
  public TagSortDirection TagOrderDirection { get; init; } =
    TagSortDirection.From(OrderDirection.ASC.ToString());
}
