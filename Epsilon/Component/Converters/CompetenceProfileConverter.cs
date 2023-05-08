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
            taskResults,
            professionalResults,
            filteredTerms,
            CalculateDecayingAverageScore(taskResults, FhictConstants.ProfessionalTasks)
        );
    }

    private static IEnumerable<DecayingAverage> CalculateDecayingAverageScore(
        IEnumerable<CompetenceOutcomeResult> results,
        IDictionary<int, ProfessionalTask> outcomes)
    {
        var listDecayingAverage = new List<DecayingAverage>();

        foreach (var outcome in outcomes)
        {
            var filtered = results.ToList()
                .FindAll(r => r.OutcomeId == outcome.Key);

            if (filtered.Count > 0)
            {
                var totalCount = 0.0;
                var recentOutcome = filtered.First();
                var pastOutcomes = filtered.GetRange(1, filtered.Count - 1);
                var endScore = 0.0;
                if (pastOutcomes.Count > 0)
                {
                    pastOutcomes.ForEach(r => { totalCount = r.Grade + totalCount; });
                    var pastScore = Math.Round(totalCount / pastOutcomes.Count, 2) * .35;
                    endScore = pastScore + (recentOutcome.Grade * .65);
                }
                else
                {
                    endScore = recentOutcome.Grade;
                }

                listDecayingAverage.Add(new DecayingAverage(
                    endScore,
                    outcome.Value.Layer,
                    outcome.Value.Activity
                ));
            }
        }

        return listDecayingAverage;
    }
}