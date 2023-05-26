namespace Epsilon.Abstractions.Component;

public interface ICompetenceComponentFetcher
{
    public Task<ICompetenceComponent> FetchUnknown(DateTime startDate, DateTime endDate);
}

public interface ICompetenceComponentFetcher<TComponent> : ICompetenceComponentFetcher
    where TComponent : ICompetenceComponent
{
    public Task<TComponent> Fetch(DateTime startDate, DateTime endDate);
}