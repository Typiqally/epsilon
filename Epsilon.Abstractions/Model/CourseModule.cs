namespace Epsilon.Abstractions.Model
{
    public class CourseModule
    {
        public string Name { get; set; } = String.Empty;
        public IEnumerable<CourseOutcome> Kpis { get; set; } = Enumerable.Empty<CourseOutcome>();
        
        public string DecayingAverage { get; set; }
    }
}
