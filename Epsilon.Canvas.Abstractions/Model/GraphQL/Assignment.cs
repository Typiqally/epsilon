using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQL;

public record Assignment(
    [property: JsonPropertyName("name")] string Name, 
    [property: JsonPropertyName("modules")] List<Module>? Modules 
);