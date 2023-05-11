using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Component;

[CompetenceComponentName("persona")]
public record PersonaPage(string PersonaHtml) : ICompetenceWordComponent
{
    public OpenXmlElement ToWord(MainDocumentPart mainDocumentPart)
    {
        var body = new Body();

        const string altChunkId = "PersonaPage";

        var personaHtmlStream = new MemoryStream(Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes($"<html>{PersonaHtml}</html>")).ToArray());
        var formatImportPart = mainDocumentPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, altChunkId);

        formatImportPart.FeedData(personaHtmlStream);
        var altChunk = new AltChunk
        {
            Id = altChunkId,
        };

        body.Append(altChunk);

        return body;
    }
}