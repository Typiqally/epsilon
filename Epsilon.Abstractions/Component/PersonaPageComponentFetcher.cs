namespace Epsilon.Abstractions.Component;

public abstract class PersonaPageComponentFetcher<TComponent> : ICompetenceComponentFetcher<TComponent> where TComponent : ICompetenceComponent
{
    public async Task<ICompetenceComponent> FetchUnknown() => await Fetch();

    public abstract Task<TComponent> Fetch();
}