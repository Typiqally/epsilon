namespace Epsilon.Abstractions.Model;

public record ProfessionalTaskOutcome(
    string ArchitectureLayer,
    string Activity,
    int MasteryLevel,
    int Grade,
    DateTime AssessedAt
);