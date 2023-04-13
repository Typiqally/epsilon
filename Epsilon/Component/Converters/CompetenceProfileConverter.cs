using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model.GraphQl;
using Epsilon.Canvas.Abstractions.QueryResponse;

namespace Epsilon.Component.Converters;

public class CompetenceProfileConverter : ICompetenceProfileConverter
{

    public CompetenceProfile ConvertFrom(GetAllUserCoursesSubmissionOutcomes getAllUserCoursesSubmissionOutcomes, IEnumerable<EnrollmentTerm> enrollmentTerms)
    {
        var professionalTaskOutcomes = new List<ProfessionalTaskOutcome>();
        var professionalSkillOutcomes = new List<ProfessionalSkillOutcome>();
        // var filteredTerms = new List<EnrollmentTerm>();
            
        

        // return filteredTerms.OrderBy(term => term.StartAt);

        foreach (var course in getAllUserCoursesSubmissionOutcomes.Data.Courses)
        {
            foreach (var submission in course.SubmissionsConnection.Nodes)
            {
                var assessmentRatings = submission.RubricAssessmentsConnection?.Nodes;

                foreach (var assessmentRating in assessmentRatings)
                {
                    foreach (var rating in assessmentRating.AssessmentRatings)
                    {
                        var outcome = rating.Outcome;

                        if (outcome != null)
                        {
                            switch (outcome.DeterminateCategory())
                            {
                                case "Task":
                                    var (architectureLayerId, activityId, tMasteryLevelId) = outcome.GetTaskDetails();
                                    if (architectureLayerId != -1 && activityId != -1 && tMasteryLevelId != -1)
                                    {
                                        if (rating.Grade() == -1) break;
                                        professionalTaskOutcomes.Add(new ProfessionalTaskOutcome(
                                            architectureLayerId,
                                            activityId,
                                            tMasteryLevelId,
                                            rating.Grade(),
                                            submission.PostedAt ?? DateTime.Now
                                        ));
                                    }
                                    break;
                                case "Skill":
                                    var (professionalSkillId, sMasteryLevelId) = outcome.GetSkillDetails();
                                    if (professionalSkillId != -1 && sMasteryLevelId != -1)
                                    {
                                        if (rating.Grade() == -1) break;
                                        professionalSkillOutcomes.Add(new ProfessionalSkillOutcome(
                                            professionalSkillId,
                                            sMasteryLevelId,
                                            rating.Grade(),
                                            submission.PostedAt ?? DateTime.Now
                                        ));
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
        
        var filteredTerms = enrollmentTerms.Where(term => term.StartAt.HasValue)
            .Where(term => professionalTaskOutcomes.Any(taskOutcome =>
                taskOutcome.AssessedAt > term.StartAt.Value && taskOutcome.AssessedAt < term.EndAt || 
                professionalSkillOutcomes.Any(skillOutcome => skillOutcome.AssessedAt > term.StartAt.Value && skillOutcome.AssessedAt < term.EndAt)))
            .Distinct();
        var sortedFilteredTerms = filteredTerms.OrderBy(term => term.StartAt);

        return new CompetenceProfile(
            HboIDomain.HboIDomain2018, 
            professionalTaskOutcomes,
            professionalSkillOutcomes,
            sortedFilteredTerms
        );
    }
}