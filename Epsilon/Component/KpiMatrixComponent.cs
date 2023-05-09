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
                .GroupBy(r => r.Outcome.Id);

            var assignments = new List<KpiMatrixAssignment>();

            var module = modules.FirstOrDefault(m => m.Name == moduleName);
            if (module == null)
            {
                module = new KpiMatrixModule(moduleName,
                    new KpiMatrix(new List<KpiMatrixOutcome>(), new List<KpiMatrixAssignment>()));
                modules.Add(module);
            }

            var moduleOutcomes = module.KpiMatrix.Outcomes.ToDictionary(o => o.Id);

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
                    module.KpiMatrix.Outcomes.Add(kpiMatrixOutcome);
                }

                var outcomeAssessments = outcomeGroup.Select(assessmentRating =>
                    new KpiMatrixOutcome(assessmentRating.Outcome.Id, assessmentRating.Outcome.Title,
                        GetGradeStatus(assessmentRating.Points, assessmentRating.Outcome.MasteryPoints)));

                var assignment = assignments.FirstOrDefault(a => a.Name == node.Assignment.Name);
                if (assignment == null)
                {
                    assignments.Add(new KpiMatrixAssignment(node.Assignment.Name, outcomeAssessments.ToList()));
                }
                else
                {
                    assignments.Remove(assignment);
                    var combinedOutcomes = assignment.Outcomes.Concat(outcomeAssessments).ToList();
                    assignments.Add(new KpiMatrixAssignment(node.Assignment.Name, combinedOutcomes));
                }
            }

            var newAssignments = module.KpiMatrix.Assignments.Concat(assignments).ToList();
            var newModule = new KpiMatrixModule(moduleName, new KpiMatrix(module.KpiMatrix.Outcomes, newAssignments));
            modules[modules.IndexOf(module)] = newModule;
        }

        return new KpiMatrixProfile(modules);
    }
}