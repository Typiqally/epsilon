namespace Epsilon.Canvas.Abstractions.Service;

public interface IFileHttpService
{
    Task<byte[]> GetFileByteArray(string url);
    HttpClient Client { get; }
}