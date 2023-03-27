namespace Epsilon.Abstractions.Http;

public abstract class HttpService
{
    protected HttpService(HttpClient client) => Client = client;

    public HttpClient Client { get; }
}