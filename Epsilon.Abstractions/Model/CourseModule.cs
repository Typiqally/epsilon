namespace Epsilon.Abstractions.Model
{
    public class CourseModule
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<CourseOutcome> Outcomes { get; set; } = Enumerable.Empty<CourseOutcome>();
        public string DecayingAverage { get; set; }
    }
}