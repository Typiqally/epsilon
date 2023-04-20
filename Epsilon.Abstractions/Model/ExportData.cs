namespace Epsilon.Abstractions.Model;

public class ExportData
{
    public string PersonaHtml { get; set; } = string.Empty;

    public IEnumerable<CourseModulePackage> CourseModules { get; set; } = Enumerable.Empty<CourseModulePackage>();
}