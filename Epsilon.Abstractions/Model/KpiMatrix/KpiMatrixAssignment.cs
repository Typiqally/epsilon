namespace Epsilon.Abstractions.Model;

public record KpiMatrixAssignment(
    string Name,
    IEnumerable<KpiMatrixOutcome> Outcomes
);