using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Component;

[CompetenceComponentName("persona")]
public record PersonaPage(string PersonaHtml) : IWordCompetenceComponent
{
    public void AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var personaHtmlBuffer = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes($"<html>{PersonaHtml}</html>")).ToArray();
        using var personaHtmlStream = new MemoryStream(personaHtmlBuffer);

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