using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record OutcomeResultLink(
    [property: JsonPropertyName("user")] string? User,
    [property: JsonPropertyName("learning_outcome")] string? Outcome,
    [property: JsonPropertyName("alignment")] string? Alignment,
    [property: JsonPropertyName("assignment")] string? Assignment);