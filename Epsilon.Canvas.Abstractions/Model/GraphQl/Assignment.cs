using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record Assignment(
    [property: JsonPropertyName("name")] string? Name, 
    [property: JsonPropertyName("htmlUrl")] Uri? HtmlUrl,
    [property: JsonPropertyName("modules")] IEnumerable<Module>? Modules ,
    [property: JsonPropertyName("rubric")] Rubric? Rubric 
);