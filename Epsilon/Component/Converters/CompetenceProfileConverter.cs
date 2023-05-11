using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model.GraphQl;
using Epsilon.Canvas.Abstractions.QueryResponse;

namespace Epsilon.Component.Converters;

public class CompetenceProfileConverter : ICompetenceProfileConverter
{
    public CompetenceProfile ConvertFrom(
        GetAllUserCoursesSubmissionOutcomes getAllUserCoursesSubmissionOutcomes,
        IHboIDomain domain,
        IEnumerable<EnrollmentTerm> enrollmentTerms)
    {
        var taskResults = new List<ProfessionalTaskResult>();
        var professionalResults = new List<ProfessionalSkillResult>();

        foreach (var course in getAllUserCoursesSubmissionOutcomes.Data.Courses)
        {
            foreach (var submissionsConnection in course.SubmissionsConnection.Nodes.Where(static s =>
                         s.PostedAt != null))
            {
                var submission = submissionsConnection.SubmissionsHistories.Nodes
                    .Where(static h => h.RubricAssessments.Nodes.Any())
                    .OrderByDescending(h => h.SubmittedAt)
                    .MaxBy(static h => h.Attempt);

                if (submission != null)
                {
                    var rubricAssessments = submission.RubricAssessments.Nodes;

                    foreach (var assessmentRating in rubricAssessments.SelectMany(static rubricAssessment =>
                                 rubricAssessment.AssessmentRatings.Where(static ar =>
                                     ar is
                                     {
                                         Points: not null, Criterion.MasteryPoints: not null,
                                         Criterion.Outcome: not null
                                     } && ar.Points >= ar.Criterion.MasteryPoints)))
                    {
                        if (FhictConstants.ProfessionalTasks.TryGetValue(assessmentRating.Criterion.Outcome.Id,
                                out var professionalTask))
                        {
                            taskResults.Add(
                                new ProfessionalTaskResult(
                                    assessmentRating.Criterion.Outcome.Id,
                                    professionalTask.Layer,
                                    professionalTask.Activity,
                                    professionalTask.MasteryLevel,
                                    assessmentRating.Points!.Value,
                                    submission.SubmittedAt!.Value
                                )
                            );
                        }
                        else if (FhictConstants.ProfessionalSkills.TryGetValue(assessmentRating.Criterion.Outcome.Id,
                                     out var professionalSkill))
                        {
                            professionalResults.Add(
                                new ProfessionalSkillResult(
                                    assessmentRating.Criterion.Outcome.Id,
                                    professionalSkill.Skill,
                                    professionalSkill.MasteryLevel,
                                    assessmentRating.Points!.Value,
                                    submission.SubmittedAt!.Value
                                )
                            );
                        }
                    }
                }
            }
        }

        var filteredTerms = enrollmentTerms
            .Where(static term => term is { StartAt: not null, EndAt: not null })
            .Where(term => taskResults.Any(taskOutcome =>
                               taskOutcome.AssessedAt >= term.StartAt && taskOutcome.AssessedAt <= term.EndAt)
                           || professionalResults.Any(skillOutcome =>
                               skillOutcome.AssessedAt > term.StartAt && skillOutcome.AssessedAt < term.EndAt))
            .Distinct()
            .OrderByDescending(static term => term.StartAt);

        ;

        return new CompetenceProfile(
            domain,
            taskResults.OrderByDescending(task => task.AssessedAt),
            professionalResults.OrderByDescending(skill => skill.AssessedAt),
            filteredTerms
        );
    }
}