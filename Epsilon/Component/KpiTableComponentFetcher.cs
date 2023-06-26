using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
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

                    if (rubricCriteria is not null)
                    {
                        foreach (var outcome in GetValidOutcomes(rubricCriteria))
                        {
                            var assessmentRatings = submission.RubricAssessments.Nodes.FirstOrDefault()?.AssessmentRatings;

                            var grade = assessmentRatings?.FirstOrDefault(ar => ar?.Criterion?.Outcome?.Id == outcome?.Id)?.Grade;

                            if (grade != null)
                            {
                                var assignmentName = submission.Assignment.Name;
                                var htmlUrl = submission.Assignment.HtmlUrl;
                                var assessmentRating = assessmentRatings?.FirstOrDefault(ar => ar?.Criterion?.Outcome?.Id == outcome?.Id);

                                if (assessmentRating is not null)
                                {
                                    var kpiTableEntryIndex = kpiTableEntries.FindIndex(kte => kte.Kpi == outcome?.Title);

                                    if (kpiTableEntryIndex > -1)
                                    {
                                        UpdateKpiTableEntry(kpiTableEntries, kpiTableEntryIndex, assignmentName, grade, htmlUrl);
                                    }
                                    else
                                    {
                                        AddKpiTableEntry(kpiTableEntries, outcome, assignmentName, grade, htmlUrl);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        kpiTableEntries = kpiTableEntries
            .OrderBy(static kte => kte.MasteryLevel.Level)
            .ThenBy(static kte => kte.Kpi)
            .ToList();
        
        return new KpiTable(kpiTableEntries);
    }
    
    private static IEnumerable<Outcome?> GetValidOutcomes(IEnumerable<Criteria> rubricCriteria)
    {
        return rubricCriteria
            .Select(static criteria => criteria.Outcome)
            .Where(static outcome => outcome is not null &&
                (FhictConstants.ProfessionalTasks.ContainsKey(outcome.Id) ||
                 FhictConstants.ProfessionalSkills.ContainsKey(outcome.Id)));
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

    private static void AddKpiTableEntry(ICollection<KpiTableEntry> kpiTableEntries, Outcome outcome, string assignmentName, string gradeStatus, Uri htmlUrl)
    {
        MasteryLevel? masteryLevel = null;
        ProfessionalSkillLevel? professionalSkill = null;
    
        if (FhictConstants.ProfessionalTasks.TryGetValue(outcome.Id, out var professionalTask)
            || FhictConstants.ProfessionalSkills.TryGetValue(outcome.Id, out professionalSkill))
        {
            masteryLevel = new HboIDomain2018().MasteryLevels.FirstOrDefault(ml => ml.Id == (professionalTask?.MasteryLevel ?? professionalSkill?.MasteryLevel));
        }

        if (masteryLevel != null)
        {
            var assignment = new KpiTableEntryAssignment(assignmentName, gradeStatus, htmlUrl);
            var kpiTableEntry = new KpiTableEntry(outcome.Title, masteryLevel, new List<KpiTableEntryAssignment> { assignment, });
            kpiTableEntries.Add(kpiTableEntry);
        }
    }
}