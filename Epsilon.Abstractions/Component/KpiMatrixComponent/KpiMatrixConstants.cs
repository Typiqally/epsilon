namespace Epsilon.Abstractions.Component.KpiMatrixComponent;

public static class KpiMatrixConstants
{
    
    public static readonly IDictionary<string, GradeStatus> GradeStatus = new Dictionary<string, GradeStatus>
    {
        {
            "Mastered", new GradeStatus("Mastered", "44F656")
        },
        {
            "Insufficient", new GradeStatus("Insufficient", "FA1818")
        },
        {
            "NotGradedAssessed", new GradeStatus("Not graded, Assignment assessed", "FAFF00")
        },
        {
            "NotGradedNotAssessed", new GradeStatus("Not graded, Assignment not assessed", "9F2B68")
        },
    };
}