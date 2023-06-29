namespace Epsilon.Abstractions.Component;

public abstract class CompetenceComponentFetcher<TComponent> : ICompetenceComponentFetcher<TComponent> where TComponent : ICompetenceComponent
{
    public async Task<ICompetenceComponent> FetchUnknown(string componentName, DateTime startDate, DateTime endDate) => await Fetch(componentName,startDate, endDate);

    public abstract Task<TComponent> Fetch(string componentName, DateTime startDate, DateTime endDate);
}