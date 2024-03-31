using StackAPI.Utils;

namespace StackAPI.Models.Domain.Common;
public class TagSortDirection : ValueOf<string, TagSortDirection> {
  protected override void Validate() {
    if (Value != OrderDirection.DESC.ToString() && Value != OrderDirection.ASC.ToString()) {
      throw new ArgumentException($"Tag sort direction must be either DESC or ASC, but got {Value}");
    }
  }
}
