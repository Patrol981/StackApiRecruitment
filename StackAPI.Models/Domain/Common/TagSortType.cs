using StackAPI.Utils;

namespace StackAPI.Models.Domain.Common;
public class TagSortType : ValueOf<string, TagSortType> {
  protected override void Validate() {
    if (Value != nameof(Tag.Count) && Value != nameof(Tag.Name) && Value != nameof(Tag.Id)) {
      throw new ArgumentException($"Tag sort by must be either Count, Name or Id, but got {Value}");
    }
  }
}
