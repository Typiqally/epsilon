using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record Module(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("items_count")] int Count,
    [property: JsonPropertyName("items")] IEnumerable<ModuleItem>? Items
);