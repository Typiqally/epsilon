using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQL;

public record RubricAssessmentsConnection(
    [property: JsonPropertyName("nodes")] List<RubricAssessmentNode>? Nodes
);