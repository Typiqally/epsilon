using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;

namespace Epsilon.Abstractions.Component;

[CompetenceComponentName("competence_profile")]
public record CompetenceProfile(
    IHboIDomain HboIDomain,
    IEnumerable<ProfessionalTaskResult> ProfessionalTaskOutcomes,
    IEnumerable<ProfessionalSkillResult> ProfessionalSkillOutcomes,
    IEnumerable<EnrollmentTerm> Terms,
    IEnumerable<DecayingAveragePerLayer> DecayingAveragesPerTask,
    IEnumerable<DecayingAveragePerSkill> DecayingAveragesPerSkill
) : ICompetenceWordComponent
{
    public OpenXmlElement ToWord(MainDocumentPart mainDocumentPart)
    {
        // TODO: This is simply an example to show the capability of the component architecture
        var body = new Body();

        foreach (var enrollmentTerm in Terms)
        {
            body.AppendChild(
                new Paragraph(
                    new Run(
                        new Text(enrollmentTerm.Name)
                    )
                )
            );
        }

        return body;
    }
}