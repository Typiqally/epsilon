using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.QueryResponse;

namespace Epsilon.Component.Converters;

public class CompetenceProfileConverter : ICompetenceProfileConverter
{
    public IEnumerable<DecayingAveragePerLayer> GetDecayingAverageTasks(IHboIDomain domain, IEnumerable<ProfessionalTaskResult> taskResults)
    {
        return domain.ArchitectureLayers.Select(layer => new DecayingAveragePerLayer(layer.Id,
            domain.Activities.Select(activity =>
            {
                var decayingAverage = taskResults
                    .Where(task => task.ArchitectureLayer == layer.Id && task.Activity == activity.Id)
                    .Aggregate<ProfessionalTaskResult, double>(0,
                        (current, outcome) => current * 0.35 + outcome.Grade * 0.65);

                return new DecayingAveragePerActivity(activity.Id, decayingAverage);
            })));
    }

    public IEnumerable<DecayingAveragePerSkill> GetDecayingAverageSkills(IHboIDomain domain, IEnumerable<ProfessionalSkillResult> skillResults)
    {
        return domain.ProfessionalSkills.Select(skill =>
        {
            var decayingAverage = skillResults.Where(outcome => outcome.Skill == skill.Id)
                .Aggregate<ProfessionalSkillResult, double>(0,
                    (current, outcome) => current * 0.35 + outcome.Grade * 0.65);

            return new DecayingAveragePerSkill(skill.Id, decayingAverage);
        });
    }

    public CompetenceProfile ConvertFrom(GetAllUserCoursesSubmissionOutcomes getAllUserCoursesSubmissionOutcomes,
        IHboIDomain domain, IEnumerable<EnrollmentTerm> enrollmentTerms)
    {
        var taskResults = new List<ProfessionalTaskResult>();
        var professionalResults = new List<ProfessionalSkillResult>();

        foreach (var course in getAllUserCoursesSubmissionOutcomes.Data.Courses)
        {
            foreach (var submissionsConnection in course.SubmissionsConnection.Nodes.Where(static s => s.PostedAt != null))
            {
                var submission = submissionsConnection.SubmissionsHistories.Nodes
                    .Where(static h => h.RubricAssessments.Nodes.Any())
                    .MaxBy(static h => h.Attempt);

                if (submission != null)
                {
                    var rubricAssessments = submission.RubricAssessments.Nodes;

                    foreach (var assessmentRating in rubricAssessments.SelectMany(static rubricAssessment => rubricAssessment.AssessmentRatings.Where(static ar => ar is { Points: not null, Criterion.MasteryPoints: not null, Criterion.Outcome: not null } && ar.Points >= ar.Criterion.MasteryPoints)))
                    {
                        if (FhictConstants.ProfessionalTasks.TryGetValue(assessmentRating.Criterion.Outcome.Id, out var professionalTask))
                        {
                            taskResults.Add(
                                new ProfessionalTaskResult(
                                    professionalTask.Layer,
                                    professionalTask.Activity,
                                    professionalTask.MasteryLevel,
                                    assessmentRating.Points!.Value,
                                    submissionsConnection.PostedAt!.Value
                                )
                            );
                        }
                        else if (FhictConstants.ProfessionalSkills.TryGetValue(assessmentRating.Criterion.Outcome.Id, out var professionalSkill))
                        {
                            professionalResults.Add(
                                new ProfessionalSkillResult(
                                    professionalSkill.Skill,
                                    professionalSkill.MasteryLevel,
                                    assessmentRating.Points!.Value,
                                    submissionsConnection.PostedAt!.Value
                                )
                            );
                        }
                    }
                }
            }
        }

        var filteredTerms = enrollmentTerms
            .Where(static term => term is {StartAt: not null, EndAt: not null})
            .Where(term => taskResults.Any(taskOutcome =>
                               taskOutcome.AssessedAt >= term.StartAt && taskOutcome.AssessedAt <= term.EndAt)
                           || professionalResults.Any(skillOutcome =>
                               skillOutcome.AssessedAt > term.StartAt && skillOutcome.AssessedAt < term.EndAt))
            .Distinct()
            .OrderBy(static term => term.StartAt);

        return new CompetenceProfile(
            domain,
            taskResults,
            professionalResults,
            filteredTerms,
            GetDecayingAverageTasks(domain, taskResults),
            GetDecayingAverageSkills(domain, professionalResults)
        );
    }
}