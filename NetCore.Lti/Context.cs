using System.Text.Json.Serialization;

namespace NetCore.Lti;

public record Context(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("label")] string Label,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("type")] IEnumerable<string> Type
);