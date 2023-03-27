using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Export;


public interface IExportDataPackager
{
    public Task<ExportData> GetExportData(IAsyncEnumerable<ModuleOutcomeResultCollection> data);
}