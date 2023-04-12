namespace Epsilon.Abstractions.Model;

public record CompetenceProfileOutcome(
    string? ArchitectureLayer,
    string? Activity,
    int? MasteryLevel,
    int? Grade,
    DateTime? AssessedAt
);