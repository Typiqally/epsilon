namespace Epsilon.Abstractions.Model;

public record KpiMatrixProfile(
    IEnumerable<KpiMatrixModule> KpiMatrixModules);