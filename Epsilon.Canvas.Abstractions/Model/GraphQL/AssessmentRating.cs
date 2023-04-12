using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQL;

public record AssessmentRating(
    [property: JsonPropertyName("points")] double? Points,
    [property: JsonPropertyName("outcome")] Outcome? Outcome
)
{
    public int? Grade()
    {
        return Points switch
        {
            0 => 0,
            3 => 3,
            4 => 4,
            5 => 5,
            _ => null
        };
    }
}