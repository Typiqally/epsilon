using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
}