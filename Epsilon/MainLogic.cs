using Epsilon.Abstractions;
using Epsilon.Abstractions.Export;
using Epsilon.Canvas.Abstractions.Data;
using Epsilon.Canvas.Abstractions.Services;

namespace Epsilon;

public class MainLogic : IMainLogic
{
    private readonly IModuleService _moduleService;
    private readonly IOutcomeService _outcomeService;
    private readonly IAssignmentService _assignmentService;
    private readonly IEnumerable<ICanvasModuleFileExporter> _fileExporters;

    public MainLogic(
        IModuleService moduleService,
        IOutcomeService outcomeService,
        IAssignmentService assignmentService,
        IEnumerable<ICanvasModuleFileExporter> fileExporters)
    {
        _moduleService = moduleService;
        _outcomeService = outcomeService;
        _assignmentService = assignmentService;
        _fileExporters = fileExporters;
    }
    
    public async Task<IEnumerable<Module>> GatherData(int courseId)
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

        var modules = (await _moduleService.All(courseId) ?? throw new InvalidOperationException()).ToList();

        foreach (var module in modules)
        {
            var items = await _moduleService.AllItems(courseId, module.Id);
            if (items != null)
            {
                var assignmentItems = items.Where(static item => item.Type == ModuleItemType.Assignment);

                foreach (var item in assignmentItems)
                {
                    if (item.ContentId != null && assignments.TryGetValue(item.ContentId.Value, out var assignment))
                    {
                        module.Assignments.Add(assignment);
                    }
                }
            }
        }

        return modules;
    }

    public void Export(IEnumerable<Module> modules, string format)
    {
        var filename = "Epsilon-Export-" + DateTime.Now.ToString("ddMMyyyyHHmmss");

        foreach (var fileExporter in _fileExporters)
        {
            if (fileExporter.CanExport(format))
            {
                fileExporter.Export(modules, filename);
                break;
            }
        }
    }
}