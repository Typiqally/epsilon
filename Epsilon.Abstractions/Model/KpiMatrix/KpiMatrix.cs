using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Model;

public record KpiMatrix(
    IEnumerable<string> Outcomes,
    IEnumerable<KpiMatrixAssignment> Assignments
);

