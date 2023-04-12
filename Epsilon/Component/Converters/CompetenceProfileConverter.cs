using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model.GraphQL;
using Epsilon.Canvas.Abstractions.QueryResponse;

namespace Epsilon.Component.Converters;

public class CompetenceProfileConverter : ICompetenceProfileConverter
{

    public CompetenceProfile ConvertFrom(GetAllUserCoursesSubmissionOutcomes getAllUserCoursesSubmissionOutcomes)
    {
        var competenceProfileOutcomes = new List<CompetenceProfileOutcome>();

        foreach (var course in getAllUserCoursesSubmissionOutcomes?.Data?.Courses)
        {
            foreach (var submission in course?.SubmissionsConnection?.Nodes)
            {
                var assessmentRatings = submission?.RubricAssessmentsConnection?.Nodes;

                foreach (var assessmentRating in assessmentRatings)
                {
                    competenceProfileOutcomes.AddRange(
                        assessmentRating.AssessmentRatings?.Select(
                            rating => ConvertFrom(rating, submission.PostedAt)));
                }
            }
        }
        
        // check each outcome for nulls, if any nulls are found, remove the outcome
        competenceProfileOutcomes.Where(outcome => 
            outcome.ArchitectureLayer == null || 
            outcome.Activity == null || 
            outcome.MasteryLevel == null ||
            outcome.Grade == null ||
            outcome.AssessedAt == null
        )
            .ToList()
            .ForEach(outcome => competenceProfileOutcomes.Remove(outcome));

        return new CompetenceProfile(
            HboIDomain.HboIDomain2018, 
            competenceProfileOutcomes
        );
    }
    
    public CompetenceProfileOutcome ConvertFrom(AssessmentRating assessmentRating, DateTime? assessedAt)
    {
        var outcome = assessmentRating.Outcome;
        var profile = new CompetenceProfileOutcome(
            outcome?.ArchitectureLayer(),
            outcome?.Activity(),
            outcome?.MasteryLevel(),
            assessmentRating.Grade(),
            assessedAt
        );
        Console.WriteLine(assessedAt);
        Console.WriteLine("--------------------");
        return profile;
    }
}