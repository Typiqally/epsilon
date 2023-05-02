namespace Epsilon.Abstractions.Model;

public abstract record OutcomeResult
{
    public int MasteryLevel { get; set; }
    public double Grade{ get; set; }
    public DateTime AssessedAt{ get; set; }
    public double DecayingAverage{ get; set; }
}