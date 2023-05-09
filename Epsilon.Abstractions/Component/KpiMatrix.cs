namespace Epsilon.Abstractions.Component;

public record KpiMatrix(
    IEnumerable<KpiMatrixOutcome> Outcomes,
    IEnumerable<KpiMatrixAssignment> Assignments
);

