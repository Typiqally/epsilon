namespace Epsilon.Abstractions.Component;

public interface ICompetenceComponentFetcher
{
    public Task<ICompetenceComponent> FetchUnknown();
}

public interface ICompetenceComponentFetcher<TComponent> : ICompetenceComponentFetcher
    where TComponent : ICompetenceComponent
{
    public Task<TComponent> Fetch();
}