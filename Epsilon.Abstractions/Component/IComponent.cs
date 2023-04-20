namespace Epsilon.Abstractions.Component;

public interface IComponent
{
    public Task<object?> FetchObject();
}

public interface IComponent<TResponse> : IComponent
{
    public Task<TResponse> Fetch();
}