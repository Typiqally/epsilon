using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record Outcome(
    [property: JsonPropertyName("_id")] int Id,
    [property: JsonPropertyName("title")] string Title
);