using StackAPI.Utils;

namespace StackAPI.Models.Domain.Common;
public class TagId : ValueOf<Guid, TagId> {
  protected override void Validate() {
    if (Value == Guid.Empty) {
      throw new ArgumentException("Tag Id cannot be empty", nameof(TagId));
    }
  }
}
