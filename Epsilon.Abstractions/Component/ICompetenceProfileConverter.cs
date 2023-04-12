using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model.GraphQL;
using Epsilon.Canvas.Abstractions.QueryResponse;

namespace Epsilon.Abstractions.Component;

public interface ICompetenceProfileConverter
{
    public CompetenceProfile ConvertFrom(GetAllUserCoursesSubmissionOutcomes getAllUserCoursesSubmissionOutcomes);
    public CompetenceProfileOutcome ConvertFrom(AssessmentRating assessmentRating, DateTime? assessedAt);
}