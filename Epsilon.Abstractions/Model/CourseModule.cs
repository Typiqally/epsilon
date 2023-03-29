namespace Epsilon.Abstractions.Model
{
    public class CourseModule
    {
        public string Name { get; set; } = String.Empty;
        public IEnumerable<CourseOutcome> Outcomes { get; set; } = Enumerable.Empty<CourseOutcome>();
    }
}