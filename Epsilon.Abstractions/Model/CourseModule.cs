namespace Epsilon.Abstractions.Model
{
    public class CourseModule
    {
        public string Name { get; set; }
        public IEnumerable<CourseOutcome> Outcomes { get; set; }
    }
}
