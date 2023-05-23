using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Component;

[CompetenceComponentName("persona")]
public record PersonaPage(string PersonaHtml) : ICompetenceWordComponent
{
    public void AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var personaHtmlBuffer = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes($"<html>{PersonaHtml}</html>")).ToArray();
        var personaHtmlStream = new MemoryStream(personaHtmlBuffer);

        var formatImportPart = mainDocumentPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html);
        formatImportPart.FeedData(personaHtmlStream);

        mainDocumentPart.Document.AppendChild(new Body(
            new AltChunk
            {
                Id = mainDocumentPart.GetIdOfPart(formatImportPart),
            }
        ));
    }
}