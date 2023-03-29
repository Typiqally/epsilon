namespace Epsilon.Abstractions.Model
{
    public class ExportData
    {
        public string PersonaHtml { get; set; } = string.Empty;
        public IEnumerable<CourseModule> CourseModules { get; set; } = Enumerable.Empty<CourseModule>();
    }
}