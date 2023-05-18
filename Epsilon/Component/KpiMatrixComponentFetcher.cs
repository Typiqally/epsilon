using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Component.KpiMatrixComponent;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model.GraphQl;
using Epsilon.Canvas.Abstractions.Service;
using Microsoft.Extensions.Configuration;

namespace Epsilon.Component;

public class KpiMatrixComponentFetcher : CompetenceComponentFetcher<KpiMatrixCollection>
{
    private const string GetUserKpiMatrixOutcomes = @"
query GetUserKpiMatrixOutcomes {
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
                      title
                    }
                    masteryPoints
                  }
                  points
                }
              }
            }
            attempt
            submittedAt
            assignment {
              name
              rubric {
                criteria {
                  outcome {
                    title
                    _id
masteryPoints
                  }
                }
              }
            }
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

    public KpiMatrixComponentFetcher(
        IGraphQlHttpService graphQlService,
        IConfiguration configuration
    )
    {
        _graphQlService = graphQlService;
        _configuration = configuration;
    }

    public override async Task<KpiMatrixCollection> Fetch()
    {
        var studentId = _configuration["Canvas:StudentId"];
        var outcomesQuery = GetUserKpiMatrixOutcomes.Replace("$studentIds", $"{studentId}", StringComparison.InvariantCultureIgnoreCase);
        var outcomes = await _graphQlService.Query<CanvasGraphQlQueryResponse>(outcomesQuery);
        return ConvertToComponent(outcomes, new DateTime(2023, 2, 1), DateTime.Now);
    }

    private static GradeStatus GetGradeStatus(double? points, double? mastery)
    {
        return points != null
            ? points >= mastery
                ? GradeStatus.Approved
                : GradeStatus.Insufficient
            : GradeStatus.NotGraded;
    }


    private static KpiMatrixCollection ConvertToComponent(
        CanvasGraphQlQueryResponse queryResponse,
        DateTime startAt,
        DateTime endAt
    )
    {
        var assignments = new List<KpiMatrixAssignment>();
        foreach (var course in queryResponse.Data!.Courses!)
        {
            foreach (var submissionsConnection in course.SubmissionsConnection!.Nodes)
            {
                var submission = submissionsConnection.SubmissionsHistories.Nodes
                    .Where(sub => sub.SubmittedAt > startAt && sub.SubmittedAt < endAt)
                    .MaxBy(static h => h.Attempt);

                if (submission != null)
                {
                    if (submission.Assignment?.Rubric != null)
                    {
                        var rubricAssessments = submission.Assignment.Rubric.Criteria;
                        var kpiMatrixOutcomes = new List<KpiMatrixOutcome>();
                        if (submission.Assignment.Rubric.Criteria != null)
                        {
                            foreach (var criteria in submission.Assignment.Rubric.Criteria)
                            {
                                if (criteria.Outcome != null)
                                {
                                    if (FhictConstants.ProfessionalTasks.TryGetValue(criteria.Outcome.Id, out var professionalTask))
                                    {
                                        kpiMatrixOutcomes.Add(new KpiMatrixOutcome(criteria.Outcome.Id, criteria.Outcome.Title,
                                            GetGradeStatus(submission.RubricAssessments?.Nodes?.FirstOrDefault()?.AssessmentRatings.FirstOrDefault(o => o.Criterion.Outcome?.Id == criteria.Outcome.Id)
                                                    ?.Points
                                                , criteria.Outcome.MasteryPoints)));
                                    }
                                    else if (FhictConstants.ProfessionalSkills.TryGetValue(criteria.Outcome.Id, out var professionalSkill))
                                    {
                                        kpiMatrixOutcomes.Add(new KpiMatrixOutcome(criteria.Outcome.Id, criteria.Outcome.Title,
                                            GetGradeStatus(submission.RubricAssessments?.Nodes?.FirstOrDefault()?.AssessmentRatings.FirstOrDefault(o => o.Criterion.Outcome?.Id == criteria.Outcome.Id)
                                                    ?.Points
                                                , criteria.Outcome.MasteryPoints)));
                                    }
                                }
                            }
                        }

                        if (kpiMatrixOutcomes.Count > 0 && submission.Assignment.Name != null)
                        {
                            var assignment = new KpiMatrixAssignment(submission.Assignment.Name, kpiMatrixOutcomes);
                            assignments.Add(assignment);
                        }
                    }
                }
            }
        }

        return new KpiMatrixCollection(assignments);
    }
}