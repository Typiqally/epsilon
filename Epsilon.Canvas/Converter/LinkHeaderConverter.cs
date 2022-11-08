using System.Text.RegularExpressions;
using Epsilon.Canvas.Abstractions.Converter;
using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Canvas.Converter;

public class LinkHeaderConverter : ILinkHeaderConverter
{
    private static readonly Regex s_relationRegex = new("(?<=rel=\").+?(?=\")", RegexOptions.IgnoreCase);
    private static readonly Regex s_linkRegex = new("(?<=<).+?(?=>)", RegexOptions.IgnoreCase);

    public LinkHeader ConvertFrom(HttpResponseMessage response)
    {
        if (!response.Headers.Contains("Link"))
        {
            throw new KeyNotFoundException("Header does not contain link key");
        }

        return ConvertFrom(response.Headers.GetValues("Link").First());
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
                var relation = relMatch.Value.ToLower();
                var link = linkMatch.Value;

                switch (relation)
                {
                    case "first":
                        linkHeader.FirstLink = link;
                        break;
                    case "prev":
                        linkHeader.PrevLink = link;
                        break;
                    case "next":
                        linkHeader.NextLink = link;
                        break;
                    case "last":
                        linkHeader.LastLink = link;
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