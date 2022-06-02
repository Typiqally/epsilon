using Epsilon.Canvas.Abstractions.Data;

namespace Epsilon.Abstractions.Export;

public interface ICanvasModuleCollectionExporter
{
    public void Export(IEnumerable<Module> modules, string format);
}