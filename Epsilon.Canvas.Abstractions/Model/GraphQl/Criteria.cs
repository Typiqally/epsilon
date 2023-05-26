using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record Criteria(
    [property: JsonPropertyName("outcome")] Outcome? Outcome
);