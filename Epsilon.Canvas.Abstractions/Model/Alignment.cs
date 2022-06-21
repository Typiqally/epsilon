using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record Alignment(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("html_url")] Uri Url
);