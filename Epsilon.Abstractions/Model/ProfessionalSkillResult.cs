namespace Epsilon.Abstractions.Model;

public record ProfessionalSkillResult(
    int OutcomeId,
    int Skill,
    int MasteryLevel,
    double Grade,
    DateTime AssessedAt
)  : CompetenceOutcomeResult(OutcomeId, Grade, AssessedAt);