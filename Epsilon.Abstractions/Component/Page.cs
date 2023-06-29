using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Component;

public record Page(string Html) : IWordCompetenceComponent
{
    public void AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        var htmlBuffer = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes($"<html>{Html}</html>")).ToArray();
        using var htmlStream = new MemoryStream(htmlBuffer);

        var formatImportPart = mainDocumentPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html);
        formatImportPart.FeedData(htmlStream);

        mainDocumentPart.Document.AppendChild(new Body(
            new AltChunk
            {
                Id = mainDocumentPart.GetIdOfPart(formatImportPart),
            }
        ));
    }
}