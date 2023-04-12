namespace Epsilon.Abstractions.Model;

public record ProfessionalSkillOutcome(
    string ProfessionalSkill,
    int MasteryLevel,
    int Grade,
    DateTime AssessedAt
);