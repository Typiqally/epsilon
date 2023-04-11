using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQL;

public record AssessmentRating(
    [property: JsonPropertyName("points")] double? Points, 
    [property: JsonPropertyName("outcome")] Outcome? Outcome
);