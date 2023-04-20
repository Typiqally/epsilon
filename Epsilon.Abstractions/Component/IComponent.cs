namespace Epsilon.Abstractions.Component;

public interface IComponent<TResponse>
{
    public Task<TResponse> Fetch();
}