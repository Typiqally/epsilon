namespace Epsilon;

public record CompetenceProfile(
    HboIDomain Domain,
    IEnumerable<CompetenceProfileKpi> ProfileKpis);