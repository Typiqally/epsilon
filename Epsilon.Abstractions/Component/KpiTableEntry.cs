namespace Epsilon.Abstractions.Component;

public record KpiTableEntry(
    string Kpi,
    KpiTableEntryLevel Level,
    IEnumerable<KpiTableEntryAssignment> Assignments
);
