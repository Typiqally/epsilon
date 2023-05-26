using System.Text.RegularExpressions;
using Epsilon.Canvas.Abstractions.Converter;
using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Converter;

public class LinkHeaderConverter : ILinkHeaderConverter
{
    private static readonly Regex s_relationRegex = new Regex("(?<=rel=\").+?(?=\")", RegexOptions.IgnoreCase);
    private static readonly Regex s_linkRegex = new Regex("(?<=<).+?(?=>)", RegexOptions.IgnoreCase);

    public LinkHeader ConvertFrom(HttpResponseMessage response)
    {
        return !response.Headers.Contains("Link")
            ? throw new KeyNotFoundException("Header does not contain link key")
            : ConvertFrom(response.Headers.GetValues("Link").First());
    }

    public LinkHeader ConvertFrom(string from)
    {
        var linkHeader = new LinkHeader();
        var linkStrings = from.Split(',');

        foreach (var linkString in linkStrings)
        {
            var relMatch = s_relationRegex.Match(linkString);
            var linkMatch = s_linkRegex.Match(linkString);

            if (relMatch.Success && linkMatch.Success)
            {
                var relation = relMatch.Value.ToUpperInvariant();
                var link = linkMatch.Value;

                switch (relation)
                {
                    case "FIRST":
                        linkHeader.FirstLink = link;
                        break;
                    case "PREV":
                        linkHeader.PrevLink = link;
                        break;
                    case "NEXT":
                        linkHeader.NextLink = link;
                        break;
                    case "LAST":
                        linkHeader.LastLink = link;
                        break;
                    default:
                        break;
                }
            }
        }

        return linkHeader;
    }

    public string ConvertTo(LinkHeader to)
    {
        throw new NotSupportedException();
    }
}