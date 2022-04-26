using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Data;

public record Outcome(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("title")] string Title
);