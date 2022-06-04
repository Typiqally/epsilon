namespace Epsilon.Abstractions.Export;

public interface IModuleExporterCollection
{
    public IDictionary<string, ICanvasModuleExporter> DetermineExporters(IEnumerable<string> formats);
}