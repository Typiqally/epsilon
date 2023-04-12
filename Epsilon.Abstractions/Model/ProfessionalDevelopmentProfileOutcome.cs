namespace Epsilon.Abstractions.Model;

public record ProfessionalDevelopmentProfileOutcome(
    string ArchitectureLayer,
    int MasteryLevel,
    int Grade,
    DateTime AssessedAt
);