using Epsilon.Abstractions.Component;
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

    public override async Task<KpiMatrixCollection> Fetch(string componentName, DateTime startDate, DateTime endDate)
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
                    var rubricCriteria = submission.Assignment.Rubric.Criteria?.ToArray();
                    var kpiMatrixOutcomes = new List<KpiMatrixOutcome>();

                    if (rubricCriteria != null)
                    {
                        foreach (var outcome in rubricCriteria.Select(static criteria => criteria.Outcome))
                        {
                            //Validate that outcome is a HboI KPI 
                            if (outcome != null
                                && (FhictConstants.ProfessionalTasks.ContainsKey(outcome.Id) || FhictConstants.ProfessionalSkills.ContainsKey(outcome.Id))
                                && rubricCriteria.Any())
                            {
                                var assessmentRatings = submission.RubricAssessments.Nodes.FirstOrDefault()?.AssessmentRatings;
                                var gradeStatus = GetGradeStatus(assessmentRatings?.FirstOrDefault(ar => ar?.Criterion?.Outcome?.Id == outcome.Id));

                                kpiMatrixOutcomes.Add(new KpiMatrixOutcome(outcome.Id, outcome.Title, KpiMatrixCollection.DefaultGradeStatus[gradeStatus]));
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

    private static OutcomeGradeStatus GetGradeStatus(AssessmentRating? rating)
    {
        if (rating != null)
        {
            if (rating.Points != null)
            {
                return rating.IsMastery
                    ? OutcomeGradeStatus.Mastered
                    : OutcomeGradeStatus.NotMastered;
            }

            return OutcomeGradeStatus.NotGraded;
        }

        return OutcomeGradeStatus.NotAssessed;
    }
}