using Epsilon.Abstractions.Model;

namespace Epsilon.Abstractions.Component;

public record KpiTableEntry(
    string Kpi,
    MasteryLevel MasteryLevel,
    IEnumerable<KpiTableEntryAssignment> Assignments
);
