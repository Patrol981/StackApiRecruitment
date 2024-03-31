using System.Text.Json.Serialization;

namespace StackAPI.Models.Responses;
public class StackApiTag {
  [JsonPropertyName("has_synonyms")] public bool HasSynonyms { get; init; }
  [JsonPropertyName("is_moderator_only")] public bool IsModeratorOnly { get; init; }
  [JsonPropertyName("is_required")] public bool IsRequired { get; init; }
  [JsonPropertyName("count")] public long Count { get; init; }
  [JsonPropertyName("name")] public string Name { get; init; } = default!;
}
