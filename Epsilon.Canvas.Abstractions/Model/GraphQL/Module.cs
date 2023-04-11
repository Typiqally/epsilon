using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQL;

public record Module(
    [property: JsonPropertyName("name")] string Name
);