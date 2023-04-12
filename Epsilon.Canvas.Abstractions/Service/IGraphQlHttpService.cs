namespace Epsilon.Canvas.Abstractions.Service;

public interface IGraphQlHttpService
{
    public Task<T?> Query<T>(string query);
}