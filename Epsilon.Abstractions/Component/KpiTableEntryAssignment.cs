namespace Epsilon.Abstractions.Component;

public record KpiTableEntryAssignment(
    string Name,
    string Grade,
    KpiTableEntryAssignmentGradeStatus GradeStatus,
    Uri Link
);