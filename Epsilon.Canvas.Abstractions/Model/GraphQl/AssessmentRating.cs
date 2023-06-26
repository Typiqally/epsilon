using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record AssessmentRating(
    [property: JsonPropertyName("criterion")] Criterion? Criterion,
    [property: JsonPropertyName("points")] double? Points
)
{
    public bool IsMastery => Points >= Criterion?.MasteryPoints;
    
    public string Grade => Points switch
    {
        >= 5.0 => "O",
        >= 4.0 => "G",
        >= 3.0 => "S",
        >= 0.0 => "U",
        _ => "-",
    };
}