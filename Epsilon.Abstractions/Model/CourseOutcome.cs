namespace Epsilon.Abstractions.Model
{
    public class CourseOutcome
    {
        public string Name { get; set; } = String.Empty;
        public IEnumerable<CourseAssignment> Assignments { get; set; } = Enumerable.Empty<CourseAssignment>();
        public string Description { get; set; } = String.Empty;
    }
}