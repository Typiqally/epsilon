namespace Epsilon.Abstractions.Component;

public abstract class EpsilonComponentFetcher<TComponent> : IEpsilonComponentFetcher<TComponent>
    where TComponent : IEpsilonComponent
{
    public async Task<object?> FetchObject()
    {
        return await Fetch();
    }

    public abstract Task<TComponent> Fetch();
}