using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Data;

public record Assignment(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("html_url")] Uri Url
);