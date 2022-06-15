using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record OutcomeResult(
    [property: JsonPropertyName("mastery")] bool? Mastery,
    [property: JsonPropertyName("score")] double? Score,
    [property: JsonPropertyName("links")] IDictionary<string, string> Links
)
{
    [JsonIgnore]
    public Outcome? Outcome { get; set; }
}