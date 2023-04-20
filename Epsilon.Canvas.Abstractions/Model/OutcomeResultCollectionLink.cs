using System.Globalization;
using System.Text.Json.Serialization;

namespace Epsilon.Canvas.Abstractions.Model;

public record OutcomeResultCollectionLink(
    [property: JsonPropertyName("outcomes")] IEnumerable<Outcome> Outcomes,
    [property: JsonPropertyName("alignments")] IEnumerable<Alignment> Alignments
)
{
    public IDictionary<string, Outcome> OutcomesDictionary => Outcomes.DistinctBy(static o => o.Id)
        .ToDictionary(static o => o.Id.ToString(CultureInfo.InvariantCulture), static o => o);

    public IDictionary<string, Alignment> AlignmentsDictionary => Alignments.DistinctBy(static a => a.Id)
        .ToDictionary(static a => a.Id, static a => a);
}