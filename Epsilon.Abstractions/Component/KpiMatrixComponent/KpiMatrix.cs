namespace Epsilon.Abstractions.Component.KpiMatrixComponent;

public record KpiMatrix(
    IEnumerable<KpiMatrixOutcome> Outcomes,
    IEnumerable<KpiMatrixAssignment> Assignments
);

