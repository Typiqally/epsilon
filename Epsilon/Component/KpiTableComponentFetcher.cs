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
                    })
                {
                    var rubricCriteria = submission.Assignment.Rubric.Criteria?.ToArray();

                    if (rubricCriteria != null)
                    {
                        foreach (var outcome in rubricCriteria.Select(static criteria => criteria.Outcome).Where(static c => c != null))
                        {
                            //Validate that outcome is a HboI KPI 
                            if (outcome != null
                                && (FhictConstants.ProfessionalTasks.ContainsKey(outcome.Id) || FhictConstants.ProfessionalSkills.ContainsKey(outcome.Id))
                                && rubricCriteria.Any())
                            {
                                var assessmentRatings = submission.RubricAssessments.Nodes.FirstOrDefault()?.AssessmentRatings;

                                if (assessmentRatings is not null)
                                {
                                    var grade = assessmentRatings.FirstOrDefault(ar => ar?.Criterion?.Outcome?.Id == outcome.Id)?.Grade;

                                    if (grade != null)
                                    {
                                        var kpiName = outcome.Title;
                                        var assignmentName = submission.Assignment.Name;
                                        var htmlUrl = submission.Assignment.HtmlUrl;
                                        var assessmentRating = assessmentRatings.FirstOrDefault(ar => ar?.Criterion?.Outcome?.Id == outcome.Id);
                                        var outcomeGradeLevel = GetMasteryLevel(assessmentRating);

                                        var kpiTableEntryIndex = kpiTableEntries.FindIndex(kte => kte.Kpi == kpiName);

                                        if (outcomeGradeLevel is not null)
                                        {
                                            if (kpiTableEntryIndex > -1)
                                            {
                                                UpdateKpiTableEntry(kpiTableEntries, kpiTableEntryIndex, assignmentName, grade, htmlUrl);
                                            }
                                            else
                                            {
                                                AddKpiTableEntry(kpiTableEntries, kpiName, outcomeGradeLevel.Value, assignmentName, grade, htmlUrl);
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
        
        kpiTableEntries = kpiTableEntries.OrderBy(static kte => kte.Kpi).ToList();
        
        return new KpiTable(kpiTableEntries, KpiTable.DefaultGradeStatus);
    }
    
    private static void UpdateKpiTableEntry(IList<KpiTableEntry> kpiTableEntries, int index, string assignmentName, string gradeStatus, Uri htmlUrl)
    {
        var existingEntry = kpiTableEntries[index];
        var updatedAssignments = existingEntry.Assignments.Append(
            new KpiTableEntryAssignment(assignmentName, gradeStatus, htmlUrl)
        );

        var updatedEntry = existingEntry with
        {
            Assignments = updatedAssignments,
        };

        kpiTableEntries[index] = updatedEntry;
    }

    private static void AddKpiTableEntry(ICollection<KpiTableEntry> kpiTableEntries, string kpiName, OutcomeGradeLevel kpiLevel, string assignmentName, string gradeStatus, Uri htmlUrl)
    {
        kpiTableEntries.Add(new KpiTableEntry(
            kpiName,
            KpiTable.DefaultGradeStatus[kpiLevel],
            new List<KpiTableEntryAssignment>
            {
                new KpiTableEntryAssignment(assignmentName, gradeStatus, htmlUrl),
            }
        ));
    }
    
    private static OutcomeGradeLevel? GetMasteryLevel(AssessmentRating? assessmentRating)
    {
        static OutcomeGradeLevel? GetGradeLevel(int masteryLevel)
        {
            return masteryLevel switch
            {
                0 => OutcomeGradeLevel.One,
                1 => OutcomeGradeLevel.Two,
                2 => OutcomeGradeLevel.Three,
                3 => OutcomeGradeLevel.Four,
                _ => null,
            };
        }

        if (assessmentRating != null)
        {
            if (FhictConstants.ProfessionalTasks.TryGetValue(assessmentRating.Criterion.Outcome.Id, out var professionalTask))
            {
                return GetGradeLevel(professionalTask.MasteryLevel);
            }

            if (FhictConstants.ProfessionalSkills.TryGetValue(assessmentRating.Criterion.Outcome.Id, out var professionalSkill))
            {
                return GetGradeLevel(professionalSkill.MasteryLevel);
            }
        }

        return null;
    }
}