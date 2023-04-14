using System.Text.Json.Serialization;

<<<<<<< HEAD
namespace Epsilon.Canvas.Abstractions.Model.GraphQL;

public record SubmissionsConnection(
    [property: JsonPropertyName("nodes")] List<Node>? Nodes
=======
namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record SubmissionsConnection(
    [property: JsonPropertyName("nodes")] List<SubmissionsConnectionNode>? Nodes
>>>>>>> f6062a3 (Link submission query with competence profile endpoint (#79))
);