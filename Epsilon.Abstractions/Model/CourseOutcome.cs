namespace Epsilon.Abstractions.Model;

public class CourseOutcome
{
    public string Name { get; set; } = string.Empty;

    public IEnumerable<CourseAssignment> Assignments { get; set; } = Enumerable.Empty<CourseAssignment>();

    public string Description { get; set; } = string.Empty;
}