namespace Epsilon.Abstractions.Component;

public record KpiMatrixAssignment(
    string Name,
    IEnumerable<KpiMatrixOutcome> Outcomes
);