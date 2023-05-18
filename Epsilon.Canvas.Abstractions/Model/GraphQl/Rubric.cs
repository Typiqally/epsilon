using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record Rubric(
    [property: JsonPropertyName("criteria")] IEnumerable<Criteria>? Criteria
    
    );