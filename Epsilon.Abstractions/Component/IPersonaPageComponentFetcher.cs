namespace Epsilon.Abstractions.Component;

public interface IPersonaPageComponentFetcher
{
    public Task<IPersonaPageComponent> FetchUnknown();
}

public interface IPersonaPageComponentFetcher<TComponent> : IPersonaPageComponentFetcher
    where TComponent : IPersonaPageComponent
{
    public Task<TComponent> Fetch();
}