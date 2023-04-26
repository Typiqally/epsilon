namespace Epsilon.Abstractions.Component;

public interface IEpsilonComponent
{
    public Task<object?> FetchObject();
}

public interface IEpsilonComponent<TResponse> : IEpsilonComponent
{
    public Task<TResponse> Fetch();
}