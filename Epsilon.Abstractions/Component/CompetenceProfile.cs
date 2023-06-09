using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Component;

public record CompetenceProfile(
    IHboIDomain HboIDomain,
    IEnumerable<ProfessionalTaskResult> ProfessionalTaskOutcomes,
    IEnumerable<ProfessionalSkillResult> ProfessionalSkillOutcomes
) : IWordCompetenceComponent
{
    public void AddToWordDocument(MainDocumentPart mainDocumentPart)
    {
        // TODO: This is simply an example to show the capability of the component architecture
        var body = new Body();

        body.AppendChild(
            new Paragraph(
                new Run(
                    new Text("Competence profile comes here")
                    )
                )
            );

        mainDocumentPart.Document.AppendChild(body);
    }
}