namespace Epsilon.Abstractions.Model;

public record ProfessionalSkillResult(
    int Skill,
    int MasteryLevel,
    double Grade,
    DateTime AssessedAt
);