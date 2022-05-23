using Epsilon.Canvas.Abstractions.Data;

namespace Epsilon.Abstractions.Format;

public interface IFileFormat
{
    IFileFormat FormatFile(IEnumerable<Module> modules);
    bool CreateDocument(string fileName);
}