using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record OutcomeResultCollection(
    [property: JsonPropertyName("outcome_results")]
    IEnumerable<OutcomeResult> OutcomeResults,
    [property: JsonPropertyName("linked")] OutcomeResultCollectionLink? Links
)
{
    public double GetDecayingAverage()
    {
        var decayingAverage = 0.0;
        
        foreach(var grade in OutcomeResults)
        {
            if (grade.Score != null)
            {
                decayingAverage = decayingAverage * 0.35 + grade.Score.Value * 0.65;
            }
        }

        return decayingAverage;
    }
}