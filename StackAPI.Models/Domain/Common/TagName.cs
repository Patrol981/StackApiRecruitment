using StackAPI.Utils;

namespace StackAPI.Models.Domain.Common;
public class TagName : ValueOf<string, TagName> {
  protected override void Validate() {
    if (Value.Length < 1) {
      throw new ArgumentException("Tag Name cannot be empty", nameof(TagName));
    }
  }
}
