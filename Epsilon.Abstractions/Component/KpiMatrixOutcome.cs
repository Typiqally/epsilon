using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Component;
public record KpiMatrixOutcome(
    string Title,
    GradeStatus GradeStatus
);