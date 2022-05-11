using Epsilon.Canvas.Abstractions.Data;

namespace Epsilon.Formats.Abstractions;

public interface IFileFormat
{
    IFileFormat FormatFile(IEnumerable<Module> modules);
    bool CreateDocument(string fileName);
}