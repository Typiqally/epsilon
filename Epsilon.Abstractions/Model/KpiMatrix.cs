using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Model;

public enum GradeStatus
{
    Approved,
    Insufficient,
    NotGraded
}
public record KpiMatrixOutcome(
    string Title,
    GradeStatus GradeStatus
);

public record KpiMatrixAssignment(
    string Name,
    IEnumerable<KpiMatrixOutcome> Outcomes
    );

public record KpiMatrix(
    IEnumerable<string> Outcomes,
    IEnumerable<KpiMatrixAssignment> Assignments
    );

