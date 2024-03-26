using System.Diagnostics.CodeAnalysis;

namespace StackAPI.Models.Requests;
public class GetTagRequest : IParsable<GetTagRequest> {
  public Guid Id { get; init; }

  public static GetTagRequest Parse(string s, IFormatProvider? provider) {
    return new GetTagRequest() { Id = Guid.Parse(s) };
  }

  public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out GetTagRequest result) {
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
