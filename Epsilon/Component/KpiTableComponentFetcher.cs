using Epsilon.Abstractions.Component;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model.GraphQl;
using Epsilon.Canvas.Abstractions.Service;
using Microsoft.Extensions.Configuration;

namespace Epsilon.Component;

public class KpiTableComponentFetcher : CompetenceComponentFetcher<KpiTable>
{
    private const string GetUserKpis = @"
        query GetUserKpis {
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
                      htmlUrl
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

    public KpiTableComponentFetcher(
        IGraphQlHttpService graphQlService,
        IConfiguration configuration
    )
    {
        _graphQlService = graphQlService;
        _configuration = configuration;
    }

    public override async Task<KpiTable> Fetch(DateTime startDate, DateTime endDate)
    {
        var studentId = _configuration["Canvas:StudentId"];
        var outcomesQuery = GetUserKpis.Replace("$studentIds", studentId, StringComparison.InvariantCultureIgnoreCase);
        var outcomes = await _graphQlService.Query<CanvasGraphQlQueryResponse>(outcomesQuery);

        var kpiTableEntries = new List<KpiTableEntry>();

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
                    }
                   )
                {
                    var rubricCriteria = submission.Assignment.Rubric.Criteria?.ToArray();

                    if (rubricCriteria != null)
                    {
                        foreach (var outcome in rubricCriteria.Select(static criteria => criteria.Outcome))
                        {
                            // Validate that outcome is a HboI KPI 
                            if (outcome != null
                                && FhictConstants.ProfessionalTasks.ContainsKey(outcome.Id)
                                && rubricCriteria.Any()) // || FhictConstants.ProfessionalSkills.ContainsKey(outcome.Id))
                            {
                                var assessmentRatings = submission.RubricAssessments.Nodes.FirstOrDefault()?.AssessmentRatings;

                                if (assessmentRatings != null)
                                {
                                    var gradeStatus = assessmentRatings
                                        .Where(ar => ar?.Criterion?.Outcome?.Id == outcome.Id)
                                        .Select(static ar => ar.Points)
                                        .FirstOrDefault();
                                    
                                    if (gradeStatus != null)
                                    {
                                        var kpiName = outcome.Title;
                                        var assignmentName = submission.Assignment.Name;
                                        var htmlUrl = submission.Assignment.HtmlUrl;
                                        var assessmentRating = assessmentRatings.FirstOrDefault(ar => ar?.Criterion?.Outcome?.Id == outcome.Id);
                                        var outcomeGradeLevel = GetMasteryLevel(assessmentRating);
                                        
                                        // Check if the KPI entry already exists
                                        var kpiTableEntryIndex = kpiTableEntries.FindIndex(kte => kte.Kpi == kpiName);

                                        if (kpiTableEntryIndex > -1)
                                        {
                                            if (outcomeGradeLevel != null)
                                            {
                                                // Add assignment to existing KPI entry
                                                kpiTableEntries[kpiTableEntryIndex].Assignments = kpiTableEntries[kpiTableEntryIndex].Assignments.Append(
                                                    new KpiTableEntryAssignment(
                                                        assignmentName, 
                                                        GetGradeStatus(gradeStatus.Value),
                                                        KpiTable.DefaultGradeStatus[outcomeGradeLevel.Value + 1],
                                                        htmlUrl
                                                    ));
                                            }
                                        }
                                        else
                                        {
                                            if (outcomeGradeLevel != null)
                                            {
                                                // Create a new KPI entry
                                                kpiTableEntries.Add(new KpiTableEntry
                                                {
                                                    Kpi = kpiName,
                                                    Assignments = new List<KpiTableEntryAssignment>
                                                    {
                                                        new KpiTableEntryAssignment(
                                                            assignmentName,
                                                            GetGradeStatus(gradeStatus.Value),
                                                            KpiTable.DefaultGradeStatus[outcomeGradeLevel.Value + 1],
                                                            htmlUrl
                                                        ),
                                                    },
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        kpiTableEntries = kpiTableEntries.OrderByDescending(static kte => kte.Assignments.Count()).ToList();

        return new KpiTable(kpiTableEntries, KpiTable.DefaultGradeStatus);
    }
    
    private static OutcomeGradeLevel? GetMasteryLevel(AssessmentRating? assessmentRating)
    {
        if (assessmentRating != null)
        {
            if (FhictConstants.ProfessionalTasks.TryGetValue(assessmentRating.Criterion.Outcome.Id, out var professionalTask))
            {
                return professionalTask.MasteryLevel switch
                {
                    1 => OutcomeGradeLevel.One,
                    2 => OutcomeGradeLevel.Two,
                    3 => OutcomeGradeLevel.Three,
                    4 => OutcomeGradeLevel.Four,
                    _ => null,
                };
            }
        }

        return null;
    }
    
    private static string GetGradeStatus(double grade)
    {
        return grade switch
        {
            >= 5.0 => "O",
            >= 4.0 => "G",
            >= 3.0 => "S",
            >= 0.0 => "U",
            _ => "-",
        };
    }
}