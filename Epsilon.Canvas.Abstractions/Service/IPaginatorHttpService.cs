namespace Epsilon.Canvas.Abstractions.Service;

public interface IPaginatorHttpService
{
    public Task<IEnumerable<T>> GetAllPages<T>(HttpMethod method, string uri);
}