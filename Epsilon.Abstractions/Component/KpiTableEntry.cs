namespace Epsilon.Abstractions.Component;

public record KpiTableEntry {
    public string Kpi { get; set; }
    public IEnumerable<KpiTableEntryAssignment> Assignments { get; set; }
}
