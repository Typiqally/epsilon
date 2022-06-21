namespace Epsilon.Abstractions.Export;

public interface IModuleExporterCollection
{
    public IEnumerable<string> Formats();

    public IDictionary<string, ICanvasModuleExporter> DetermineExporters(IEnumerable<string> formats);
}