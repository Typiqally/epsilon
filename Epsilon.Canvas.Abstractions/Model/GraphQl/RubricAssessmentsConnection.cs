using System.Text.Json.Serialization;

<<<<<<< HEAD
namespace Epsilon.Canvas.Abstractions.Model.GraphQL;
=======
namespace Epsilon.Canvas.Abstractions.Model.GraphQl;
>>>>>>> f6062a3 (Link submission query with competence profile endpoint (#79))

public record RubricAssessmentsConnection(
    [property: JsonPropertyName("nodes")] List<RubricAssessmentNode>? Nodes
);