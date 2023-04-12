namespace Epsilon.Abstractions.Model;

public record ProfessionalSkillOutcome(
    int ProfessionalSkillId,
    int MasteryLevelId,
    int Grade,
    DateTime AssessedAt
);