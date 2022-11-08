using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record OutcomeResult(
    [property: JsonPropertyName("mastery")]
    bool? Mastery,
    [property: JsonPropertyName("score")] double? Score,
    [property: JsonPropertyName("links")] OutcomeResultLink Link
)
{
    public string? Grade()
    {
        return Score switch
        {
            <= 2 => "Unsatisfactory",
            3 => "Satisfactory",
            4 => "Good",
            5 => "Outstanding",
            _ => null,
        };
    }
}