using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQL;

public record Outcome(
    [property: JsonPropertyName("title")] string? Title
);