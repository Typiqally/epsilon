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

        var kpiTableEntries = new Dictionary<int, KpiTableEntry>() { };

        foreach (var course in outcomes.Data!.Courses!)
        {
            foreach (var submission in course.SubmissionsConnection!.Nodes.Select(sm => sm.SubmissionsHistories.Nodes
                                                                                          .Where(sub => sub.SubmittedAt > startDate && sub.SubmittedAt < endDate)
                                                                                          .MaxBy(static h => h.Attempt)))
            {
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
                                var assignmentName = submission!.Assignment!.Name!;
                                var htmlUrl = submission.Assignment.HtmlUrl;
                                var assessmentRating = assessmentRatings?.FirstOrDefault(ar => ar?.Criterion?.Outcome?.Id == outcome?.Id);

                                if (assessmentRating is not null)
                                {

                                    if (kpiTableEntries.ContainsKey(outcome.Id))
                                    {
                                        UpdateKpiTableEntry(kpiTableEntries, outcome, submission.Assignment, grade);
                                    }
                                    else
                                    {
                                        AddKpiTableEntry(kpiTableEntries, outcome, submission.Assignment, grade);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
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

    private static void UpdateKpiTableEntry(Dictionary<int, KpiTableEntry> kpiTableEntries, Outcome outcome, Assignment assignment, string gradeStatus)
    {
        var kpiEntry = kpiTableEntries.GetValueOrDefault(outcome.Id);
        var updatedAssignments = kpiEntry.Assignments.Append(
            new KpiTableEntryAssignment(assignment.Name, gradeStatus, assignment.HtmlUrl)
        );
        var updatedEntry = kpiEntry with { Assignments = updatedAssignments, };

        kpiTableEntries.Remove(outcome.Id);
        kpiTableEntries.Add(outcome.Id, updatedEntry);
    }

    private static void AddKpiTableEntry(Dictionary<int, KpiTableEntry> kpiTableEntries, Outcome outcome, Assignment assignment, string gradeStatus)
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
            var kpiAssignment = new KpiTableEntryAssignment(assignment.Name, gradeStatus, assignment.HtmlUrl);
            var kpiTableEntry = new KpiTableEntry(outcome.Title, masteryLevel, new List<KpiTableEntryAssignment> { kpiAssignment, });
            kpiTableEntries.Add(outcome.Id, kpiTableEntry);
        }
    }
}