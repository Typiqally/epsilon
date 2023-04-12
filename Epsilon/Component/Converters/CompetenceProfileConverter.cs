using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model.GraphQL;
using Epsilon.Canvas.Abstractions.QueryResponse;

namespace Epsilon.Component.Converters;

public class CompetenceProfileConverter : ICompetenceProfileConverter
{

    public CompetenceProfile ConvertFrom(GetAllUserCoursesSubmissionOutcomes getAllUserCoursesSubmissionOutcomes)
    {
        var professionalTaskOutcomes = new List<ProfessionalTaskOutcome>();
        var professionalSkillOutcomes = new List<ProfessionalSkillOutcome>();

        // ProfessionalTaskOutcomes
        foreach (var course in getAllUserCoursesSubmissionOutcomes?.Data?.Courses)
        {
            foreach (var submission in course?.SubmissionsConnection?.Nodes)
            {
                var assessmentRatings = submission?.RubricAssessmentsConnection?.Nodes;

                foreach (var assessmentRating in assessmentRatings)
                {
                    professionalTaskOutcomes.AddRange(
                        assessmentRating.AssessmentRatings?.Select(
                            rating => ConvertFrom(rating, submission.PostedAt)));
                }
            }
        }
        
        // ProfessionalSkillOutcomes
        
        
        // check each outcome for nulls, if any nulls are found, remove the outcome
        professionalTaskOutcomes.Where(outcome => 
            outcome.ArchitectureLayer == null || 
            outcome.Activity == null || 
            outcome.MasteryLevel == null ||
            outcome.Grade == null ||
            outcome.AssessedAt == null
        ).ToList().ForEach(outcome => professionalTaskOutcomes.Remove(outcome));

        return new CompetenceProfile(
            HboIDomain.HboIDomain2018, 
            professionalTaskOutcomes,
            professionalSkillOutcomes
        );
    }
    
    public ProfessionalTaskOutcome ConvertFrom(AssessmentRating assessmentRating, DateTime? assessedAt)
    {
        var outcome = assessmentRating.Outcome;
        return new ProfessionalTaskOutcome(
            outcome?.ArchitectureLayer(),
            outcome?.Activity(),
            outcome?.MasteryLevel(),
            assessmentRating.Grade(),
            assessedAt
        );
    }
}