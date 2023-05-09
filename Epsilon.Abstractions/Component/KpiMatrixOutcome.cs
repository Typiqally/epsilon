using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Component;
public record KpiMatrixOutcome(
    int Id,
    string Title,
    GradeStatus GradeStatus
);