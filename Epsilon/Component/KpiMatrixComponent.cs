using DocumentFormat.OpenXml.Spreadsheet;
using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions.QueryResponse;
using Epsilon.Canvas.Abstractions.Service;
using Microsoft.Extensions.Configuration;

namespace Epsilon.Component;

public class KpiMatrixComponent : Component<KpiMatrixProfile>
{
    private readonly IConfiguration _configuration;
    private readonly IGraphQlHttpService _graphQlService;
    private readonly IAccountHttpService _accountHttpService;

    public KpiMatrixComponent(
        IGraphQlHttpService graphQlService,
        IAccountHttpService accountHttpService,
        IConfiguration configuration
    )
    {
        _graphQlService = graphQlService;
        _accountHttpService = accountHttpService;
        _configuration = configuration;
    }

    public async override Task<KpiMatrixProfile> Fetch()
    {
        var courseId = _configuration["Canvas:courseId"];

        var outcomesQuery = QueryConstants.GetUserKpiMatrixOutcomes.Replace("$courseId", $"{courseId}");
        var outcomes = await _graphQlService.Query<GetUserKpiMatrixOutcomes>(outcomesQuery);

        var modules = ConvertToComponent(outcomes);

        return modules;
    }

    private GradeStatus GetGradeStatus(double? points, double mastery)
    {
        if (points == null)
        {
            return GradeStatus.NotGraded;
        }
        else if (points >= mastery)
        {
            return GradeStatus.Approved;
        }
        else
        {
            return GradeStatus.Insufficient;
        }
    }


    private KpiMatrixProfile ConvertToComponent(GetUserKpiMatrixOutcomes getUserKpiMatrixOutcomes)
    {
        var modules = new List<KpiMatrixModule>();

        foreach (var node in getUserKpiMatrixOutcomes.Data?.Course.SubmissionsConnection?.Nodes)
        {
            var moduleName = node.Assignment.Modules.FirstOrDefault()?.Name;
            if (moduleName == null) continue;

            var outcomeGroups = node.RubricAssessmentsConnection.Nodes
                .Where(n => n?.AssessmentRatings != null)
                .SelectMany(n => n.AssessmentRatings)
                .Where(r => r.Outcome != null)
                .GroupBy(r => r.Outcome.Title);

            var existingOutcomes = new HashSet<string>();
            var outcomes = new List<string>();
            var assignments = new List<KpiMatrixAssignment>();

            foreach (var outcomeGroup in outcomeGroups)
            {
                var outcomeTitle = outcomeGroup.Key;
                if (existingOutcomes.Contains(outcomeTitle)) continue;

                existingOutcomes.Add(outcomeTitle);
                outcomes.Add(outcomeTitle);

                var outcomeAssessments = outcomeGroup.Select(assessmentRating =>
                    new KpiMatrixOutcome(assessmentRating.Outcome.Title,
                        GetGradeStatus(assessmentRating.Points, assessmentRating.Outcome.MasteryPoints)));

                var assignment = assignments.FirstOrDefault(a => a.Name == node.Assignment.Name);
                if (assignment == null)
                {
                    assignments.Add(new KpiMatrixAssignment(node.Assignment.Name, outcomeAssessments));
                }
                else
                {
                    assignments.Remove(assignment);
                    var combinedOutcomes = assignment.Outcomes.Concat(outcomeAssessments).ToList();
                    assignments.Add(new KpiMatrixAssignment(node.Assignment.Name, combinedOutcomes));
                }

            }

            var module = modules.FirstOrDefault(m => m.Name == moduleName);
            if (module == null)
            {
                module = new KpiMatrixModule(moduleName, new KpiMatrix(outcomes.Distinct(), assignments));
                modules.Add(module);
            }
            else
            {
                var moduleIndex = modules.IndexOf(module);
                if (moduleIndex == -1)
                {
                    modules.Add(new KpiMatrixModule(moduleName, new KpiMatrix(outcomes.Distinct(), assignments)));
                }
                else
                {
                    var existingModule = modules[moduleIndex];
                    var newAssignments = existingModule.KpiMatrix.Assignments.Concat(assignments);
                    var newOutcomes = existingModule.KpiMatrix.Outcomes.Concat(outcomes).Distinct();
                    var newModule = new KpiMatrixModule(moduleName, new KpiMatrix(newOutcomes, newAssignments));
                    modules[moduleIndex] = newModule;
                }
            }
        }

        return new KpiMatrixProfile(modules);
    }
}