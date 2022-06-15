namespace Epsilon.Canvas.Abstractions.Service;

public interface IPaginatorService
{
    public Task<IEnumerable<T>> FetchAll<T>(HttpMethod method, string uri);
}