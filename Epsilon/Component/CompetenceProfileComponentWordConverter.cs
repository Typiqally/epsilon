using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Epsilon.Abstractions.Component;

namespace Epsilon.Component;

public class CompetenceProfileComponentWordConverter : EpsilonComponentConverter<OpenXmlElement, CompetenceProfile>
{
    public override Task<OpenXmlElement> Convert(CompetenceProfile component)
    {
        // TODO: This is simply an example to show the capability of the component architecture
        var body = new Body();

        foreach (var enrollmentTerm in component.Terms)
        {
            body.AppendChild(
                new Paragraph(
                    new Run(
                        new Text(enrollmentTerm.Name)
                    )
                )
            );
        }

        return Task.FromResult<OpenXmlElement>(body);
    }
}