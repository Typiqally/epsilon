using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Model;

public record KpiMatrix(
    List<KpiMatrixOutcome> Outcomes,
    List<KpiMatrixAssignment> Assignments
);

