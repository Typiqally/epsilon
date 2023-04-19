using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record Module(
    [property: JsonPropertyName("name")] string Name
);