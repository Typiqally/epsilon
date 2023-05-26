using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record AssessmentRating(
    [property: JsonPropertyName("criterion")] Criterion? Criterion,
    [property: JsonPropertyName("points")] double? Points
)
{
    public bool IsMastery => Points >= Criterion?.MasteryPoints;
}