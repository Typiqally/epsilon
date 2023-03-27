namespace Epsilon.Abstractions.Http;

public abstract class HttpService
{
    protected HttpService(HttpClient client) => Client = client;

    protected HttpClient Client { get; }
}