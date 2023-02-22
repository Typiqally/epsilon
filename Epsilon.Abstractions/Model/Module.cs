namespace Epsilon.Abstractions.Model
{
    public class Module
    {
        public string Name { get; set; }
        public IEnumerable<Kpi> Kpis { get; set; }
    }
}
