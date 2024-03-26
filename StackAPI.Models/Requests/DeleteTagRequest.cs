using System.Diagnostics.CodeAnalysis;

namespace StackAPI.Models.Requests;
public class DeleteTagRequest : IParsable<DeleteTagRequest> {
  public Guid Id { get; init; }

  public static DeleteTagRequest Parse(string s, IFormatProvider? provider) {
    return new DeleteTagRequest() { Id = Guid.Parse(s) };
  }

  public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out DeleteTagRequest result) {
    result = null;
    if (s == null) {
      return false;
    }
    try {
      result = Parse(s, provider);
      return true;
    } catch {
      return false;
    }
  }
}
