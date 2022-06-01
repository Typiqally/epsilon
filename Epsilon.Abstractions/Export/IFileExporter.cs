namespace Epsilon.Abstractions.Export;

public interface IFileExporter<in T>
{
    public bool CanExport(string format);

    void Export(T data, string path);
}