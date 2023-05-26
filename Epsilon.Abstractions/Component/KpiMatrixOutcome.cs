
namespace Epsilon.Abstractions.Component;
public record KpiMatrixOutcome(
    int Id,
    string Title,
    KpiMatrixOutcomeGradeStatus KpiMatrixOutcomeGradeStatus
);