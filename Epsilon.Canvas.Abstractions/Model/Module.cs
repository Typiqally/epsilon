using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record Module(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("items_count")] int Count)
{
    [JsonIgnore]
    public IList<Submission> Submissions { get; set; } = new Collection<Submission>();

    public bool HasSubmissions() => Submissions.Count > 0;
}