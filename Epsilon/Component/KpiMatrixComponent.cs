using Epsilon.Abstractions.Component;
using Epsilon.Abstractions.Model;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions.QueryResponse;
using Epsilon.Canvas.Abstractions.Service;
using Microsoft.Extensions.Configuration;

namespace Epsilon.Component;

public class KpiMatrixComponent : Component<KpiMatrix>
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
    
    public async override Task<KpiMatrix> Fetch()
    {
        var courseId = _configuration["Canvas:courseId"];

        var outcomesQuery = QueryConstants.GetUserKpiMatrixOutcomes.Replace("$courseId", $"{courseId}");
        var outcomes = await _graphQlService.Query<GetUserKpiMatrixOutcomes>(outcomesQuery);
        
        var kpiMatrix = ConvertToComponent(outcomes);

        return kpiMatrix;
    }

    private GradeStatus GetGradeStatus(double? points)
    {
        return points switch
        {
            null => GradeStatus.NotGraded,
            >= 4 => GradeStatus.Approved,
            _ => GradeStatus.Insufficient
        };
    }
    
    private KpiMatrix ConvertToComponent(GetUserKpiMatrixOutcomes getUserKpiMatrixOutcomes)
    {
        var outcomes = new List<string>();
        var assignments = new List<KpiMatrixAssignment>();

        if (getUserKpiMatrixOutcomes.Data?.Course.SubmissionsConnection?.Nodes == null)
            return new KpiMatrix(outcomes, assignments);
        foreach (var node in getUserKpiMatrixOutcomes.Data?.Course.SubmissionsConnection?.Nodes)
        {
            var assignmentOutcomes = new List<KpiMatrixOutcome>();
            var assessmentRatings = node.RubricAssessmentsConnection.Nodes.FirstOrDefault()
                ?.AssessmentRatings;
            if (assessmentRatings == null) continue;
            
            foreach (var assessmentRating in assessmentRatings)
                {
                    if (assessmentRating.Outcome != null)
                    {
                        assignmentOutcomes.Add(new KpiMatrixOutcome(assessmentRating.Outcome.Title,
                            GetGradeStatus(assessmentRating.Points)));
                        if (outcomes.All(o => o != assessmentRating.Outcome.Title))
                        {
                            outcomes.Add(assessmentRating.Outcome.Title);
                        }
                    }
                }

            assignments.Add(new KpiMatrixAssignment(node.Assignment.Name, assignmentOutcomes));
        }

        return new KpiMatrix(outcomes ,assignments);
    }
}