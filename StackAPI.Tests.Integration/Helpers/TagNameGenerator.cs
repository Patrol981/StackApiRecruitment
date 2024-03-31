namespace StackAPI.Tests.Integration.Helpers;
public static class TagNameGenerator {
  public static string[] GetTagNames(int count) {
    var names = new string[count];

    for (int i = 0; i < count; i++) {
      names[i] = $"exampleTag_{i}";
    }

    return names;
  }
}
