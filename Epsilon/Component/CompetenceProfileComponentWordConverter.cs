using DocumentFormat.OpenXml;
using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;

namespace Epsilon.Component;

public class CompetenceProfileComponentWordConverter : IComponentWordConverter<CompetenceProfile>
{
    public Task<OpenXmlElement> Convert(CompetenceProfile competenceProfile)
    {
        throw new NotImplementedException();
    }
}