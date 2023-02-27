using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Export;

public interface IModuleDataPackager
{
    public Task<IEnumerable<CourseModule>> GetExportData(IAsyncEnumerable<ModuleOutcomeResultCollection> data);
}