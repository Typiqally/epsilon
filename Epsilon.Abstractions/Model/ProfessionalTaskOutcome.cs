namespace Epsilon.Abstractions.Model;

public record ProfessionalTaskOutcome(
    int ArchitectureLayerId,
    int ActivityId,
    int MasteryLevelId,
    int Grade,
    DateTime AssessedAt
);