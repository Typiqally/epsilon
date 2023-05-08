namespace Epsilon.Abstractions.Model;

public record ProfessionalTaskResult(
    int OutcomeId,
    int ArchitectureLayer,
    int Activity,
    int MasteryLevel,
    double Grade,
    DateTime AssessedAt
) : CompetenceOutcomeResult(OutcomeId, Grade, AssessedAt);