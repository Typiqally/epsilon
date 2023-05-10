namespace Epsilon.Abstractions.Component;

public abstract class ComponentFetcher<TComponent> : IComponentFetcher<TComponent> where TComponent : IEpsilonComponent
{
    public async Task<IEpsilonComponent> FetchUnknown() => await Fetch();

    public abstract Task<TComponent> Fetch();
}