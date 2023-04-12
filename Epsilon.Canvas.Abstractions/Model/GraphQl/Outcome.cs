using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record Outcome(
    [property: JsonPropertyName("title")] string? Title
)
{
    public string? ArchitectureLayer()
    {
        return Title?.Split("-").Last()[..1] switch 
        {
            "S" => "Software",
            "H" => "Hardware Interfacing",
            "I" => "Infrastructure",
            "O" => "Organisational Processes",
            "U" => "User Interaction",
            _ => null
        };
    }
    
    public string? Activity()
    {
        return Title?.Split("-").First() switch
        {
            "Analysis" => "Analysis",
            "Advise" => "Advice",
            "Design" => "Design",
            "Manage&Control" => "Manage & Control",
            "Realisation" => "Realisation",
            _ => null
        };
    }
    
    public int? MasteryLevel()
    {
        return Title?.Split("-").Last()[1..2] switch
        {
            "1" => 1,
            "2" => 2,
            "3" => 3,
            "4" => 4,
            _ => null
        };
    }
}