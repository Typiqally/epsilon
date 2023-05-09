namespace Epsilon.Abstractions.Model;

public record KpiMatrixAssignment(
    string Name,
    List<KpiMatrixOutcome> Outcomes
);