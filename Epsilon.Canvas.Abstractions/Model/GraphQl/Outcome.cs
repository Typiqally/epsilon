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
            _ => null
        };
    }

    public Tuple<int, int, int> GetTaskDetails()
    {
        int architectureLayerId = Title?.Split("-").Last()[..1] switch 
        {
            "H" => 0,
            "I" => 1,
            "O" => 2,
            "U" => 3,
            "S" => 4,
            _ => -1
        };
        
        int activityId = Title?.Split("-").First() switch
        {
            "Manage&Control" => 0,
            "Analysis" => 1,
            "Advise" => 2,
            "Design" => 3,
            "Realisation" => 4,
            _ => -1
        };
        
        int masteryLevelId = Title?.Split("-").Last()[1..2] switch
        {
            "1" => 0,
            "2" => 1,
            "3" => 2,
            "4" => 3,
            _ => -1
        };
        
        return new Tuple<int, int, int>(architectureLayerId, activityId, masteryLevelId);
    }
    
    public Tuple<int, int> GetSkillDetails()
    { 
        int professionalSkillId = Title?.Split("-").First() switch
        {
            "FOO" => 0,
            "IPS" => 1,
            "PL" => 2,
            "TI" => 3,
            _ => -1
        };
        
        int masteryLevelId = Title?.Split("-").Last()[..1] switch
        {
            "1" => 0,
            "2" => 1,
            "3" => 2,
            "4" => 3,
            _ => -1
        };
        
        return new Tuple<int, int>(professionalSkillId, masteryLevelId);
    }
    
    
    // public int ArchitectureLayer()
    // {
    //     // E.g. "Manage&Control-H2.1" > "H2.1" > "H"
    //     return Title?.Split("-").Last()[..1] switch 
    //     {
    //         "H" => 0,
    //         "I" => 1,
    //         "O" => 2,
    //         "U" => 3,
    //         "S" => 4,
    //         _ => -1
    //     };
    // }
    //
    // public int Skill()
    // {
    //     // E.g. "FOO-2.1" > "FOO"
    //     return Title?.Split("-").First() switch
    //     {
    //         "FOO" => 0,
    //         "IPS" => 1,
    //         "PL" => 2,
    //         "TI" => 3,
    //         _ => -1
    //     };
    // }
    //
    // public int Activity()
    // {
    //     // E.g. "Manage&Control-H2.1" > "Manage&Control"
    //     return Title?.Split("-").First() switch
    //     {
    //         "Manage&Control" => 0,
    //         "Analysis" => 1,
    //         "Advise" => 2,
    //         "Design" => 3,
    //         _ => -1
    //     };
    // }
    //
    // public int MasteryLevel()
    // {
    //     if (IsTask())
    //     {
    //         // E.g. "Manage&Control-H2.1" > "H2.1" > "2"
    //         return Title?.Split("-").Last()[1..2] switch
    //         {
    //             "1" => 0,
    //             "2" => 1,
    //             "3" => 2,
    //             "4" => 3,
    //             _ => -1
    //         };
    //     }
    //
    //     // E.g. "FOO-2.1" > "2"
    //     return Title?.Split("-").Last()[..1] switch
    //     {
    //         "1" => 0,
    //         "2" => 1,
    //         "3" => 2,
    //         "4" => 3,
    //         _ => -1
    //     };
    // }
    //
    // public bool IsValid()
    // {
    //     return ArchitectureLayer() != -1 && Activity() != -1 && MasteryLevel() != -1;
    // }
}