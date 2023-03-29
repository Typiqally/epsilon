namespace Epsilon.Canvas.Abstractions.Service;

public interface IFileHttpService
{
    Task<IEnumerable<byte>?> GetFileByteArray(string url);
}