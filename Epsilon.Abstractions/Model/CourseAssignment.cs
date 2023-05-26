namespace Epsilon.Abstractions.Model;

public class CourseAssignment
{
    public string Name { get; set; } = string.Empty;

    public string Score { get; set; } = string.Empty;

    public Uri? Url { get; set; }
}