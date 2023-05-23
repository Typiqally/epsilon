using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model.GraphQl;
using Epsilon.Canvas.Abstractions.Service;
using Microsoft.Extensions.Configuration;

namespace Epsilon.Component;

public class CompetenceProfileComponentFetcher : CompetenceComponentFetcher<CompetenceProfile>
{
    private const string GetAllUserCoursesSubmissionOutcomes = @"
        query MyQuery {
          allCourses {
            submissionsConnection(studentIds: $studentIds) {
              nodes {
                submissionHistoriesConnection {
                  nodes {
                    rubricAssessmentsConnection {
                      nodes {
                        assessmentRatings {
                          criterion {
                            outcome {
                              _id
                            }
                            masteryPoints
                          }
                          points
                        }
                      }
                    }
                    attempt
                    submittedAt
                  }
                }
                postedAt
              }
            }
          }
        }
    ";

    private readonly IConfiguration _configuration;
    private readonly IGraphQlHttpService _graphQlService;
    private readonly IAccountHttpService _accountHttpService;

    public CompetenceProfileComponentFetcher(
        IGraphQlHttpService graphQlService,
        IAccountHttpService accountHttpService,
        IConfiguration configuration
    )
    {
        _graphQlService = graphQlService;
        _accountHttpService = accountHttpService;
        _configuration = configuration;
    }

    public override async Task<CompetenceProfile> Fetch(DateTime startDate, DateTime endDate)
    {
        var studentId = _configuration["Canvas:StudentId"];
        var outcomesQuery = GetAllUserCoursesSubmissionOutcomes.Replace("$studentIds", $"{studentId}", StringComparison.InvariantCulture);

        var outcomes = await _graphQlService.Query<CanvasGraphQlQueryResponse>(outcomesQuery);
        var terms = await _accountHttpService.GetAllTerms(1);

        var competenceProfile = ConvertToComponent(outcomes, new HboIDomain2018(), terms);

        return competenceProfile;
    }

    private static CompetenceProfile ConvertToComponent(
        CanvasGraphQlQueryResponse queryResponse,
        IHboIDomain domain,
        IEnumerable<EnrollmentTerm> enrollmentTerms
    )
    {
        var taskResults = new List<ProfessionalTaskResult>();
        var professionalResults = new List<ProfessionalSkillResult>();

        if (queryResponse.Data != null)
        {
            foreach (var course in queryResponse.Data.Courses)
            {
                foreach (var submissionsConnection in course.SubmissionsConnection.Nodes)
                {
                    var submission = submissionsConnection.SubmissionsHistories.Nodes
                        .Where(static h => h.RubricAssessments.Nodes.Any())
                        .MaxBy(static h => h.Attempt);

                    if (submission != null)
                    {
                        var rubricAssessments = submission.RubricAssessments.Nodes;

                        foreach (var assessmentRating in rubricAssessments.SelectMany(static rubricAssessment => rubricAssessment.AssessmentRatings.Where(static ar =>
                                     ar is { Points: not null, Criterion.MasteryPoints: not null, Criterion.Outcome: not null, } && ar.Points >= ar.Criterion.MasteryPoints)))
                        {
                            if (FhictConstants.ProfessionalTasks.TryGetValue(assessmentRating.Criterion.Outcome.Id, out var professionalTask))
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
                            else if (FhictConstants.ProfessionalSkills.TryGetValue(assessmentRating.Criterion.Outcome.Id, out var professionalSkill))
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
        }

        var filteredTerms = enrollmentTerms
            .Where(static term => term is { StartAt: not null, EndAt: not null, })
            .Where(term => taskResults.Any(taskOutcome =>
                               taskOutcome.AssessedAt >= term.StartAt && taskOutcome.AssessedAt <= term.EndAt)
                           || professionalResults.Any(skillOutcome =>
                               skillOutcome.AssessedAt > term.StartAt && skillOutcome.AssessedAt < term.EndAt))
            .Distinct()
            .OrderByDescending(static term => term.StartAt);

        return new CompetenceProfile(
            domain,
            taskResults.OrderByDescending(static r => r.SubmittedAt),
            professionalResults.OrderByDescending(static r => r.SubmittedAt),
            filteredTerms
        );
    }
}