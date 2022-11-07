using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record OutcomeResult(
    [property: JsonPropertyName("mastery")]
    bool? Mastery,
    [property: JsonPropertyName("score")] double? Score,
    [property: JsonPropertyName("links")] OutcomeResultLink Link
)
{
    public string Grade()
    {
        switch (Score)
        {
            default:
                return "Unsatisfactory";
            case 3:
                return "Satisfactory";
            case 4:
                return "Good";
            case 5:
                return "Outstanding";
        }
    }
}