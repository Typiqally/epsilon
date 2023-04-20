namespace Epsilon.Abstractions.Model;

public class CourseModulePackage
{
    public string Name { get; set; } = string.Empty;

    public IEnumerable<CourseOutcome> Outcomes { get; set; } = Enumerable.Empty<CourseOutcome>();
}