namespace Epsilon.Abstractions.Component;

public interface IEpsilonComponentFetcher
{
    public Task<object?> FetchObject();
}

public interface IEpsilonComponentFetcher<TResponse> : IEpsilonComponentFetcher
{
    public Task<TResponse> Fetch();
}