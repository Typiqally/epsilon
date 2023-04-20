using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Epsilon.Canvas.Abstractions.Model;

public record Outcome(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("description")] string Description
)
{
    public string ShortDescription()
    {
        var description = RemoveHtml();

        // Function gives only the short English description back of the outcome.
        var startPos = description.IndexOf(" EN ", StringComparison.Ordinal) + " EN ".Length;
        var endPos = description.IndexOf(" NL ", StringComparison.Ordinal);

        return description[startPos..endPos];
    }

    private string RemoveHtml()
    {
        var raw = Regex.Replace(Description, "<.*?>", " ");
        var trimmed = Regex.Replace(raw, @"\s\s+", " ");

        return trimmed;
    }
}