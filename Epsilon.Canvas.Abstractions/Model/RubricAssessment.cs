using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record RubricAssessment(
    [property: JsonPropertyName("score")] double? Score,
    [property: JsonPropertyName("artifact_attempt")] int Attempt,
    [property: JsonPropertyName("data")] IEnumerable<RubricRating> Ratings
);