using Epsilon.Canvas.Abstractions.Service;
using Epsilon.Canvas.Http;

namespace Epsilon.Canvas.Service;

public class FileHttpService : HttpService, IFileHttpService
{
    public FileHttpService(HttpClient client)
        : base(client)
    {
    }

    public async Task<IEnumerable<byte>?> GetFileByteArray(Uri url)
    {
        return await Client.GetByteArrayAsync(url);
    }
}