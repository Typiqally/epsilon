using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.Data;
using Epsilon.Canvas.Abstractions.Services;

namespace Epsilon.Canvas;

public class CanvasModuleCollectionFetcher : ICanvasModuleCollectionFetcher
{
    private readonly IModuleService _moduleService;
    private readonly IOutcomeService _outcomeService;
    private readonly IAssignmentService _assignmentService;

    public CanvasModuleCollectionFetcher(
        IModuleService moduleService,
        IOutcomeService outcomeService,
        IAssignmentService assignmentService)
    {
        _moduleService = moduleService;
        _outcomeService = outcomeService;
        _assignmentService = assignmentService;
    }

    public async Task<IEnumerable<Module>> Fetch(int courseId)
    {
        var assignments = await FetchAssignmentsAndOutcomes(courseId);
        var modules = await AddAssignmentsToModules(courseId, assignments);

        return modules;
    }

    private async Task<Dictionary<int, Assignment>> FetchAssignmentsAndOutcomes(int courseId)
    {
        var outcomeResults = await _outcomeService.AllResults(courseId) ?? throw new InvalidOperationException();
        var masteredOutcomeResults = outcomeResults.Where(static result => result.Mastery.HasValue && result.Mastery.Value);

        var assignments = new Dictionary<int, Assignment>();
        var outcomes = new Dictionary<int, Outcome>();

        foreach (var outcomeResult in masteredOutcomeResults)
        {
            var outcomeId = int.Parse(outcomeResult.Links["learning_outcome"]);
            if (!outcomes.TryGetValue(outcomeId, out var outcome))
            {
                outcome = await _outcomeService.Find(outcomeId) ?? throw new InvalidOperationException();
                outcomes.Add(outcomeId, outcome);
            }

            outcomeResult.Outcome = outcome;

            var assignmentId = int.Parse(outcomeResult.Links["assignment"]["assignment_".Length..]);
            if (!assignments.TryGetValue(assignmentId, out var assignment))
            {
                assignment = await _assignmentService.Find(courseId, assignmentId) ?? throw new InvalidOperationException();
                assignments.Add(assignmentId, assignment);
            }

            assignment.OutcomeResults.Add(outcomeResult);
        }

        return assignments;
    }

    private async Task<List<Module>> AddAssignmentsToModules(int courseId, IReadOnlyDictionary<int, Assignment> assignments)
    {
        var modules = (await _moduleService.All(courseId) ?? throw new InvalidOperationException()).ToList();

        foreach (var module in modules)
        {
            var items = await _moduleService.AllItems(courseId, module.Id);
            if (items == null) continue;

            var assignmentItems = items.Where(static item => item.Type == ModuleItemType.Assignment);

            foreach (var item in assignmentItems)
            {
                if (item.ContentId != null && assignments.TryGetValue(item.ContentId.Value, out var assignment))
                {
                    module.Assignments.Add(assignment);
                }
            }
        }

        return modules;
    }
}