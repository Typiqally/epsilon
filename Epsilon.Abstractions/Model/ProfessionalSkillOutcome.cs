namespace Epsilon.Abstractions.Model;

public record ProfessionalSkillOutcome(
    int ProfessionalSkillId,
    int MasteryLevel,
    int Grade,
    DateTime AssessedAt
);