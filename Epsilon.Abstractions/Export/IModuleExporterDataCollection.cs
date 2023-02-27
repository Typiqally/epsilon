using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Export;

public interface IModuleExporterDataCollection
{
    public Task<IAsyncEnumerable<CourseModule>> GetExportData(IAsyncEnumerable<ModuleOutcomeResultCollection> data);
}