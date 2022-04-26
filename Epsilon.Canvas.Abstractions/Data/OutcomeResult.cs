using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Data;

public record OutcomeResult(
    [property: JsonPropertyName("mastery")] bool? Mastery,
    [property: JsonPropertyName("links")] IDictionary<string, string> Links
);