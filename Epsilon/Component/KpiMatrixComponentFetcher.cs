using Epsilon.Abstractions.Component;
using Epsilon.Canvas.Abstractions.Model;
using Epsilon.Canvas.Abstractions.Model.GraphQl;
using Epsilon.Canvas.Abstractions.Service;
using Microsoft.Extensions.Configuration;

namespace Epsilon.Component;

public class KpiMatrixComponentFetcher : ComponentFetcher<KpiMatrixCollection>
{
    private const string GetUserKpiMatrixOutcomes = @"
        query GetUserKpiMatrixOutcomes {
          allCourses {
            submissionsConnection(studentIds: $studentIds) {
              nodes {
                assignment {
                  name
                  modules {
                    name
                  }
                }
                rubricAssessmentsConnection {
                  nodes {
                    assessmentRatings {
                      points
                      outcome {
                        _id
                        title
                        masteryPoints
                      }
                    }
                  }
                }
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

        return ConvertToComponent(outcomes);
    }

    private static GradeStatus GetGradeStatus(double? points, double mastery)
    {
        return points != null
            ? points >= mastery
                ? GradeStatus.Approved
                : GradeStatus.Insufficient
            : GradeStatus.NotGraded;
    }


    private static KpiMatrixCollection ConvertToComponent(CanvasGraphQlQueryResponse getUserKpiMatrixOutcomes)
    {
        var modules = new List<KpiMatrixModule>();

        foreach (var course in getUserKpiMatrixOutcomes.Data?.Courses)
        {
            foreach (var node in course.SubmissionsConnection.Nodes)
            {
                var moduleName = node.Assignment.Modules.FirstOrDefault()?.Name;
                if (moduleName == null)
                {
                    continue;
                }

                var outcomeGroups = node.RubricAssessmentsConnection.Nodes
                    .Where(static n => n.AssessmentRatings != null)
                    .SelectMany(static n => n.AssessmentRatings)
                    .Where(static r => r.Outcome != null)
                    .GroupBy(static r => r.Outcome.Id);

                var assignments = new List<KpiMatrixAssignment>();

                var module = modules.FirstOrDefault(m => m.Name == moduleName);
                if (module == null)
                {
                    module = new KpiMatrixModule(moduleName,
                        new KpiMatrix(new List<KpiMatrixOutcome>(), new List<KpiMatrixAssignment>()));
                    modules.Add(module);
                }

                var moduleOutcomes = module.KpiMatrix.Outcomes.ToDictionary(static o => o.Id);
                var outcomes = module.KpiMatrix.Outcomes.ToList();

                foreach (var outcomeGroup in outcomeGroups)
                {
                    var outcomeId = outcomeGroup.Key;

                    // Add outcome to moduleOutcomes if it doesn't exist
                    if (!moduleOutcomes.ContainsKey(outcomeId))
                    {
                        var outcome = outcomeGroup.FirstOrDefault(x => x.Outcome.Id == outcomeId).Outcome;
                        var kpiMatrixOutcome = new KpiMatrixOutcome(outcome.Id, outcome.Title,
                            GetGradeStatus(0, outcome.MasteryPoints));
                        moduleOutcomes[outcomeId] = kpiMatrixOutcome;

                        outcomes.Add(kpiMatrixOutcome);
                    }

                    var outcomeAssessments = outcomeGroup.Select(static assessmentRating =>
                        new KpiMatrixOutcome(assessmentRating.Outcome.Id, assessmentRating.Outcome.Title,
                            GetGradeStatus(assessmentRating.Points, assessmentRating.Outcome.MasteryPoints)));

                    var assignment = assignments.FirstOrDefault(a => a.Name == node.Assignment.Name);
                    var outcomeAssessmentsList = outcomeAssessments.ToList();
                    if (assignment == null)
                    {
                        assignments.Add(new KpiMatrixAssignment(node.Assignment.Name, outcomeAssessmentsList));
                    }
                    else
                    {
                        assignments.Remove(assignment);
                        var combinedOutcomes = assignment.Outcomes.Concat(outcomeAssessmentsList).ToList();
                        assignments.Add(new KpiMatrixAssignment(node.Assignment.Name, combinedOutcomes));
                    }
                }

                var newAssignments = module.KpiMatrix.Assignments.Concat(assignments).ToList();
                var newModule = new KpiMatrixModule(moduleName, new KpiMatrix(outcomes, newAssignments));
                modules[modules.IndexOf(module)] = newModule;
            }
        }

        return new KpiMatrixCollection(modules);
    }
}