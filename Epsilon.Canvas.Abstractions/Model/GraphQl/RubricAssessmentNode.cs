using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record RubricAssessmentNode(
    [property: JsonPropertyName("assessmentRatings")] IReadOnlyList<AssessmentRating> AssessmentRatings
);