﻿using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.QueryResponse;

namespace Epsilon.Component.Converters;

public class CompetenceProfileConverter : ICompetenceProfileConverter
{
    public CompetenceProfile ConvertFrom(GetAllUserCoursesSubmissionOutcomes getAllUserCoursesSubmissionOutcomes, IEnumerable<EnrollmentTerm> enrollmentTerms)
    {
        var taskResults = new List<ProfessionalTaskResult>();
        var professionalResults = new List<ProfessionalSkillResult>();

        foreach (var course in getAllUserCoursesSubmissionOutcomes.Data.Courses)
        {
            foreach (var submission in course.SubmissionsConnection.Nodes)
            {
                var assessmentRatings = submission.RubricAssessmentsConnection?.Nodes;

                foreach (var assessmentRating in assessmentRatings)
                {
                    foreach (var (points, outcome) in assessmentRating.AssessmentRatings.Where(ar => ar is { Points: not null, Outcome: not null } && ar.Points >= ar.Outcome.MasteryPoints))
                    {
                        if (FhictConstants.ProfessionalTasks.TryGetValue(outcome!.Id, out var professionalTask))
                        {
                            taskResults.Add(
                                new ProfessionalTaskResult(
                                    professionalTask.Layer,
                                    professionalTask.Activity,
                                    professionalTask.MasteryLevel,
                                    points!.Value,
                                    submission.PostedAt!.Value
                                )
                            );
                        }
                        else if (FhictConstants.ProfessionalSkills.TryGetValue(outcome.Id, out var professionalSkill))
                        {
                            professionalResults.Add(
                                new ProfessionalSkillResult(
                                    professionalSkill.Skill,
                                    professionalSkill.MasteryLevel,
                                    points!.Value,
                                    submission.PostedAt!.Value
                                )
                            );
                        }
                    }
                }
            }
        }

        var filteredTerms = enrollmentTerms
            .Where(static term => term is { StartAt: not null, EndAt: not null })
            .Where(term => taskResults.Any(taskOutcome => taskOutcome.AssessedAt >= term.StartAt && taskOutcome.AssessedAt <= term.EndAt)
                           || professionalResults.Any(skillOutcome => skillOutcome.AssessedAt > term.StartAt && skillOutcome.AssessedAt < term.EndAt))
            .Distinct()
            .OrderBy(static term => term.StartAt);


        return new CompetenceProfile(
            new HboIDomain2018(),
            taskResults,
            professionalResults,
            filteredTerms
        );
    }
}