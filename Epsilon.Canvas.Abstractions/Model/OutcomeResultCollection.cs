using System.Text.Json.Serialization;
using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Abstractions.Response;

public record OutcomeResultCollection(
    [property: JsonPropertyName("outcome_results")] IEnumerable<OutcomeResult> OutcomeResults,
    [property: JsonPropertyName("linked")] OutcomeResultCollectionLink? Links
);