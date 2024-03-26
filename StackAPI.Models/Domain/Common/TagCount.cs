using StackAPI.Utils;

namespace StackAPI.Models.Domain.Common;
public class TagCount : ValueOf<decimal, TagCount> {
  protected override void Validate() {
    if (Value < 0) {
      throw new ArgumentException("Tag Count cannot be less than 0", nameof(TagCount));
    }
  }
}
