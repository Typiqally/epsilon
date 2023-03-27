namespace Epsilon.Abstractions.Model
{
    public class ExportData
    {
        public IEnumerable<CourseModule> CourseModules { get; set; } = Enumerable.Empty<CourseModule>();
    }
}
