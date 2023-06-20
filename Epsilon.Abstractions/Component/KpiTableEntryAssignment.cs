namespace Epsilon.Abstractions.Component;

public record KpiTableEntryAssignment(
    string Name,
    string Grade,
    KpiTableEntryAssignmentGradeStatus GradeStatus,
    Uri Link
)
{
    public void SetGrade(double grade) => Grade = grade.ToString("0.00");
}