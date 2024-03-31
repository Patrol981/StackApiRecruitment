using System.Diagnostics.CodeAnalysis;

namespace StackAPI.Models.Requests;
public class GetTagNamedRequest : IParsable<GetTagNamedRequest> {
  public string Name { get; init; } = default!;

  public static GetTagNamedRequest Parse(string s, IFormatProvider? provider) {
    return new GetTagNamedRequest() { Name = s };
  }

  public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out GetTagNamedRequest result) {
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
