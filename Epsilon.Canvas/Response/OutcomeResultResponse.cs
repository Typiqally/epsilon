using System.Text.Json.Serialization;
using Epsilon.Canvas.Abstractions.Data;

namespace Epsilon.Canvas.Response;

public record OutcomeResultResponse(
    [property: JsonPropertyName("outcome_results")] IEnumerable<OutcomeResult> OutcomeResults
);