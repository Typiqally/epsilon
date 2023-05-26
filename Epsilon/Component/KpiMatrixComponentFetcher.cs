using Epsilon.Abstractions.Component;
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

    public override async Task<KpiMatrixCollection> Fetch(DateTime startDate, DateTime endDate)
    {
        var studentId = _configuration["Canvas:StudentId"];
        var outcomesQuery = GetUserKpiMatrixOutcomes.Replace("$studentIds", $"{studentId}", StringComparison.InvariantCultureIgnoreCase);
        var outcomes = await _graphQlService.Query<CanvasGraphQlQueryResponse>(outcomesQuery);


        var assignments = new List<KpiMatrixAssignment>();
        foreach (var course in outcomes.Data!.Courses!)
        {
            foreach (var submissionsConnection in course.SubmissionsConnection!.Nodes)
            {
                var submission = submissionsConnection.SubmissionsHistories.Nodes
                    .Where(sub => sub.SubmittedAt > startDate && sub.SubmittedAt < endDate)
                    .MaxBy(static h => h.Attempt);

                if (submission is
                    {
                        Assignment.Rubric: not null,
                        RubricAssessments.Nodes: not null,
                    })
                {
                    var rubricAssessments = submission.Assignment.Rubric.Criteria?.ToArray();
                    var kpiMatrixOutcomes = new List<KpiMatrixOutcome>();

                    if (rubricAssessments != null)
                    {
                        foreach (var criteria in rubricAssessments)
                        {
                            //Validate that outcome is a HboI KPI 
                            if (criteria.Outcome != null
                                && (FhictConstants.ProfessionalTasks.ContainsKey(criteria.Outcome.Id) || FhictConstants.ProfessionalSkills.ContainsKey(criteria.Outcome.Id))
                                && rubricAssessments.Any())
                            {
                                var assessmentRatings = submission.RubricAssessments.Nodes?.FirstOrDefault()?.AssessmentRatings;

                                kpiMatrixOutcomes.Add(new KpiMatrixOutcome(criteria.Outcome.Id, criteria.Outcome.Title,
                                    GetGradeStatus(assessmentRatings != null, assessmentRatings?.FirstOrDefault(o => o?.Criterion?.Outcome?.Id == criteria?.Outcome?.Id)
                                            ?.Points
                                        , criteria.Outcome.MasteryPoints)));
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

        return new KpiMatrixCollection(assignments, KpiMatrixCollection.DefaultGradeStatus);
    }

    private static KpiMatrixOutcomeGradeStatus GetGradeStatus(bool isSubmitted, double? points, double? mastery)
    {
        var gradeStatuses = KpiMatrixCollection.DefaultGradeStatus;

        if (isSubmitted)
        {
            if (points != null)
            {
                return points >= mastery
                    ? gradeStatuses["Mastered"]
                    : gradeStatuses["Insufficient"];
            }

            return gradeStatuses["NotGradedAssessed"];
        }

        return gradeStatuses["NotGradedNotAssessed"];
    }
}