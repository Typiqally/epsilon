using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record AssessmentRating(
    [property: JsonPropertyName("criterion")] Criterion? Criterion,
    [property: JsonPropertyName("points")] double? Points
)
{
    public bool IsMastery => Points >= Criterion?.MasteryPoints;
    
    public string? Grade => Points switch
    {
        >= 5.0 => "Outstanding",
        >= 4.0 => "Good",
        >= 3.0 => "Sufficient",
        >= 0.0 => "Insufficient",
        _ => null,
    };
}