using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model.GraphQl;

public record Outcome(
    [property: JsonPropertyName("title")] string? Title
)
{
    public string? DeterminateCategory()
    {
        var outcomeTitle = Title?.Split("-").First();
        return outcomeTitle switch
        {
            "Manage&Control" => "Task",
            "Analysis" => "Task",
            "Advise" => "Task",
            "Design" => "Task",
            "Realisation" => "Task",
            "FOO" => "Skill",
            "IPS" => "Skill",
            "PL" => "Skill",
            "TI" => "Skill",
            _ => null,
        };
    }

    public Tuple<int, int, int> GetTaskDetails()
    {
        var architectureLayerId = Title?.Split("-").Last()[..1] switch
        {
            "H" => 0,
            "I" => 1,
            "O" => 2,
            "U" => 3,
            "S" => 4,
            _ => -1,
        };

        var activityId = Title?.Split("-").First() switch
        {
            "Manage&Control" => 0,
            "Analysis" => 1,
            "Advise" => 2,
            "Design" => 3,
            "Realisation" => 4,
            _ => -1,
        };

        var masteryLevelId = Title?.Split("-").Last()[1..2] switch
        {
            "1" => 0,
            "2" => 1,
            "3" => 2,
            "4" => 3,
            _ => -1,
        };

        return new Tuple<int, int, int>(architectureLayerId, activityId, masteryLevelId);
    }

    public Tuple<int, int> GetSkillDetails()
    {
        var professionalSkillId = Title?.Split("-").First() switch
        {
            "FOO" => 0,
            "IPS" => 1,
            "PL" => 2,
            "TI" => 3,
            _ => -1,
        };

        var masteryLevelId = Title?.Split("-").Last()[..1] switch
        {
            "1" => 0,
            "2" => 1,
            "3" => 2,
            "4" => 3,
            _ => -1,
        };

        return new Tuple<int, int>(professionalSkillId, masteryLevelId);
    }
}