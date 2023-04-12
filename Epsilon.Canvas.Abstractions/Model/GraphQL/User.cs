using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQL;

public record User(
    [property: JsonPropertyName("name")] string? Name
);