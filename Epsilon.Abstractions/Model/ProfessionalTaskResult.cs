namespace Epsilon.Abstractions.Model;

public record ProfessionalTaskResult(
    int ArchitectureLayer,
    int Activity,
    int MasteryLevel,
    double Grade,
    DateTime AssessedAt
);