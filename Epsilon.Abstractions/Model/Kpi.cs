namespace Epsilon.Abstractions.Model
{
    public class Kpi
    {
        public string Name { get; set; }
        public IEnumerable<Assignment> Assignments { get; set; }
    }
}
