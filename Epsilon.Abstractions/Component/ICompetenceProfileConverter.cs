using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.QueryResponse;

namespace Epsilon.Abstractions.Component;

public interface ICompetenceProfileConverter : IComponentConverter<CompetenceProfile>
{
    public CompetenceProfile ConvertToComponent(
        GetAllUserCoursesSubmissionOutcomes getAllUserCoursesSubmissionOutcomes, 
        IHboIDomain domain, 
        IEnumerable<EnrollmentTerm> terms
    );
}