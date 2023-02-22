using Epsilon.Canvas.Abstractions.Model;
using Module = Epsilon.Abstractions.Model.Module;

namespace Epsilon.Abstractions.Export;

public interface IModuleExporterDataCollection
{
    public Task<IEnumerable<Module>> GetExportData(IAsyncEnumerable<ModuleOutcomeResultCollection> data);
}