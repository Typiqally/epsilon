using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Export;

public interface ICanvasModuleExporter : IExporter<IAsyncEnumerable<ModuleOutcomeResultCollection>>
{
    
}