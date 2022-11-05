using System.Text.RegularExpressions;
using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Helpers;

public class OutcomeHelper
{
    public static string ShortenOutcomeDescription(Outcome outcome)
    {
        return ShortDescription(ConvertHtmlToRaw(outcome));
    }
    
    private static string ShortDescription(string description)
    {
        //Function gives only the short English description back of the outcome. 
        var startPos = description.IndexOf(" EN ", StringComparison.Ordinal) + " EN ".Length;
        var endPos = description.IndexOf(" NL ", StringComparison.Ordinal);

        return description.Substring(startPos, endPos - startPos);
    }

    private static string ConvertHtmlToRaw(Outcome outcome)
    {
        var raw = Regex.Replace(outcome.Description, "<.*?>", " ");
        var trimmed = Regex.Replace(raw, @"\s\s+", " ");

        return trimmed;
    }

    public static string OutcomeToText(OutcomeResult? result)
    {
        switch (result?.Score)
        {
            default:
            case 0:
                return "Unsatisfactory";
                break;
            case 3:
                return "Satisfactory";
                break;
            case 4:
                return "Good";
                break;
            case 5:
                return "Outstanding";
                break;
        }
    }
}