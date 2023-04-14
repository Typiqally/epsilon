using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record AssessmentRating(
    [property: JsonPropertyName("points")] double? Points,
    [property: JsonPropertyName("outcome")] Outcome? Outcome
);