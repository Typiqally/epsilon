using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record RubricRating(
    [property: JsonPropertyName("points")] double? Points,
    [property: JsonPropertyName("learning_outcome_id")] int? OutcomeId
)
{
    [JsonIgnore]
    public Outcome? Outcome { get; set; }
}