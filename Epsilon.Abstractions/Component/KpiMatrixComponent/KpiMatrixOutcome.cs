using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Component.KpiMatrixComponent;
public record KpiMatrixOutcome(
    int Id,
    string Title,
    GradeStatus GradeStatus
);