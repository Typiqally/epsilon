namespace Epsilon.Abstractions.Export;

public interface IExporter<in T>
{
    public IEnumerable<string> Formats { get; }

    Task<Stream> Export(T data, string format);
}