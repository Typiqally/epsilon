using Epsilon.Abstractions.Export;
using Epsilon.Canvas;
using Epsilon.Canvas.Abstractions.Data;
using Epsilon.Canvas.Abstractions.Services;
using Epsilon.Export;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Epsilon.Cli;

public class Startup : IHostedService
{
    private readonly ILogger<Startup> _logger;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ExportSettings _exportSettings;
    private readonly CanvasSettings _canvasSettings;
    private readonly IModuleService _moduleService;
    private readonly IOutcomeService _outcomeService;
    private readonly IAssignmentService _assignmentService;
    private readonly IEnumerable<ICanvasModuleFileExporter> _fileExporters;

    public Startup(
        ILogger<Startup> logger,
        IHostApplicationLifetime lifetime,
        IOptions<CanvasSettings> canvasSettings,
        IOptions<ExportSettings> exportSettings,
        IModuleService moduleService,
        IOutcomeService outcomeService,
        IAssignmentService assignmentService,
        IEnumerable<ICanvasModuleFileExporter> fileExporters)
    {
        _logger = logger;
        _lifetime = lifetime;
        _canvasSettings = canvasSettings.Value;
        _exportSettings = exportSettings.Value;
        _moduleService = moduleService;
        _outcomeService = outcomeService;
        _assignmentService = assignmentService;
        _fileExporters = fileExporters;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Epsilon, targeting course: {CourseId}", _canvasSettings.CourseId);
        _logger.LogInformation("Using export format: {format}", _exportSettings.Format);

        _lifetime.ApplicationStarted.Register(() => Task.Run(ExecuteAsync, cancellationToken));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task ExecuteAsync()
    {
        var outcomeResults = await _outcomeService.AllResults(_canvasSettings.CourseId) ?? throw new InvalidOperationException();
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
                assignment = await _assignmentService.Find(_canvasSettings.CourseId, assignmentId) ?? throw new InvalidOperationException();
                assignments.Add(assignmentId, assignment);
            }

            assignment.OutcomeResults.Add(outcomeResult);
        }

        var modules = (await _moduleService.All(_canvasSettings.CourseId) ?? throw new InvalidOperationException()).ToList();

        foreach (var module in modules)
        {
            var items = await _moduleService.AllItems(_canvasSettings.CourseId, module.Id);
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

        Export(modules);

        _lifetime.StopApplication();
    }

    private void Export(IEnumerable<Module> modules)
    {
        var filename = "Epsilon-Export-" + DateTime.Now.ToString("ddMMyyyyHHmmss");
        var format = _exportSettings.Format.ToLower();

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