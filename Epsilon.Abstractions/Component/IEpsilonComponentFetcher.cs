namespace Epsilon.Abstractions.Component;

public interface IEpsilonComponentFetcher
{
    public Task<object?> FetchObject();
}

public interface IEpsilonComponentFetcher<TComponent> : IEpsilonComponentFetcher
{
    public Task<TComponent> Fetch();
}