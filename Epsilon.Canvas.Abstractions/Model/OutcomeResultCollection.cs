using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record OutcomeResultCollection(
    [property: JsonPropertyName("outcome_results")]
    IEnumerable<OutcomeResult> OutcomeResults,
    [property: JsonPropertyName("linked")] OutcomeResultCollectionLink? Links
);