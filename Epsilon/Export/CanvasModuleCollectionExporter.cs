using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Data;

namespace Epsilon.Export;

public class CanvasModuleCollectionExporter : ICanvasModuleCollectionExporter
{
    private readonly IEnumerable<ICanvasModuleFileExporter> _fileExporters;

    public CanvasModuleCollectionExporter(IEnumerable<ICanvasModuleFileExporter> fileExporters)
    {
        _fileExporters = fileExporters;
    }

    public void Export(IEnumerable<Module> modules, string format)
    {
        var filename = "Epsilon-Export-" + DateTime.Now.ToString("ddMMyyyyHHmmss");

        foreach (var fileExporter in _fileExporters)
        {
            if (fileExporter.CanExport(format))
            {
                fileExporter.Export(modules, filename);
                break;
            }
        }
    }
}