namespace Epsilon.Abstractions.Component.KpiMatrixComponent;

public record KpiMatrixAssignment(
    string Name,
    IEnumerable<KpiMatrixOutcome> Outcomes
);