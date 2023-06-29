namespace Epsilon.Abstractions.Component;

public interface ICompetenceComponentFetcher
{
    public Task<ICompetenceComponent> FetchUnknown(string componentName, DateTime startDate, DateTime endDate);
}

public interface ICompetenceComponentFetcher<TComponent> : ICompetenceComponentFetcher
    where TComponent : ICompetenceComponent
{
    public Task<TComponent> Fetch(string componentName, DateTime startDate, DateTime endDate);
}