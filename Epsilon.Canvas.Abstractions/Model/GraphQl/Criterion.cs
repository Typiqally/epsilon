using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record Criterion(
    [property: JsonPropertyName("masteryPoints")] double? MasteryPoints,
    [property: JsonPropertyName("outcome")] Outcome? Outcome
);