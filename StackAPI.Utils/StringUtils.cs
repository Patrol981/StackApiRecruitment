namespace StackAPI.Utils;
public static class StringUtils {
  public static bool IsNullOrEmpty(string? str) {
    return str != null && str != string.Empty;
  }
}
