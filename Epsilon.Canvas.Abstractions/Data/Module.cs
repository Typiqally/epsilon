using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Data;

public record Module(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("items_count")] int Count)
{
    [JsonIgnore]
    public IList<Assignment> Assignments { get; set; } = new Collection<Assignment>();
}