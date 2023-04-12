using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record User(
    [property: JsonPropertyName("name")] string Name
);