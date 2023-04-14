using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record SubmissionsHistoriesConnection(
    [property: JsonPropertyName("nodes")] IReadOnlyList<SubmissionsHistoriesConnectionNode> Nodes
);