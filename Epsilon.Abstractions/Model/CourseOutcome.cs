namespace Epsilon.Abstractions.Model
{
    public class CourseOutcome
    {
        public string Name { get; set; }
        public IEnumerable<CourseAssignment> Assignments { get; set; }
        public string Description { get; set; }
    }
}