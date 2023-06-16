using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Epsilon.Abstractions.Component;

public record KpiTable(
    IEnumerable<KpiTableEntry> Entries
) : IWordCompetenceComponent
{
    public void AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        // TODO: This is simply an example to show the capability of the component architecture
        var body = new Body();
        
        body.AppendChild(
            new Paragraph(
                new Run(
                    new Text("Kpi table")
                )
            )
        );

        mainDocumentPart.Document.AppendChild(body);
    }
}