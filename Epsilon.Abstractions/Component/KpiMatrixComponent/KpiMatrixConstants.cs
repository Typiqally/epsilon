namespace Epsilon.Abstractions.Component.KpiMatrixComponent;

public static class KpiMatrixConstants
{
    
    public static readonly IDictionary<string, GradeStatus> GradeStatus = new Dictionary<string, GradeStatus>
    {
        {
            "Approved", new GradeStatus("Approved", "44F656")
        },
        {
            "Insufficient", new GradeStatus("Insufficient", "FA1818")
        },
        {
            "NotGraded", new GradeStatus("NotGraded", "FAFF00")
        },
    };
}