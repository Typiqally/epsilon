using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record SubmissionsConnection(
    [property: JsonPropertyName("nodes")] IReadOnlyCollection<SubmissionsConnectionNode>? Nodes
);